using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrickPile.Core.Routing.Trie;
using Raven.Client.Documents.Session;

namespace BrickPile.Core.Extensions
{
    /// <summary>
    ///     Provides BrickPile <see cref="IDocumentSession" /> helper methods
    /// </summary>
    public static class DocumentSessionExtensions
    {
        /// <summary>
        ///     Gets the start page for BrickPile
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">StructureInfo</exception>
        public static IPage GetStartPage(this IAdvancedSessionOperations session)
        {
            try
            {
                var context = new BrickPileContext(HttpContext.Current.Request.RequestContext);
                if (context.NavigationContext.StartPage != null)
                {
                    return context.NavigationContext.StartPage;
                }

                Trie trie = context.Trie;

                if (trie == null)
                {
                    throw new NullReferenceException("The RouteData DataToken does not contains the Trie object.");
                }

                return ((DocumentSession) session).Load<Page>(trie.RootNode.PageId);
            }
            catch (ArgumentNullException exception) {}
            return null;
        }

        /// <summary>
        ///     Gets the children for specific a specific page
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">StructureInfo</exception>
        public static IEnumerable<IPage> GetChildrenFor(this IAdvancedSessionOperations session, IPage page)
        {
            try
            {
                var context = new BrickPileContext(HttpContext.Current.Request.RequestContext);
                if (context.NavigationContext != null && context.NavigationContext.OpenPages != null)
                {
                    return context.NavigationContext.OpenPages.Where(x => x.Parent != null && x.Parent.Id == page.Id);
                }

                Trie trie = context.Trie;
                if (trie == null)
                {
                    throw new NullReferenceException("The RouteData DataToken does not contains the Trie object");
                }

                IEnumerable<TrieNode> nodes = trie.RootNode.Flatten(n => n.Children);

                TrieNode node = nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(page.Id));

                return node != null
                    ? ((DocumentSession) session).Load<IPage>(node.Children.Select(n => n.PageId).ToArray()) //TODO handle null values
                        .Values
                        .OrderBy(p => p.Metadata.SortOrder)
                    : Enumerable.Empty<IPage>();
            }
            catch (ArgumentNullException exception) {}
            return null;
        }

        /// <summary>
        ///     Gets the parent for a specific page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static T GetParentFor<T>(this IAdvancedSessionOperations session, IPage page) where T : IPage
        {
            if (page.Parent == null)
            {
                return default(T);
            }

            try
            {
                var context = new BrickPileContext(HttpContext.Current.Request.RequestContext);
                if (context.NavigationContext.OpenPages.Any(x => x.Id == page.Parent.Id))
                {
                    return (T) context.NavigationContext.OpenPages.Single(x => x.Id == page.Parent.Id);
                }
            }
            catch (ArgumentNullException exception) {}

            return page.Parent == null ? default(T) : ((DocumentSession) session).Load<T>(page.Parent.Id);
        }

        /// <summary>
        ///     Gets the ancestors for a specific page
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <param name="includeStartPage">if set to <c>true</c> [include start page].</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">StructureInfo</exception>
        public static IEnumerable<IPage> GetAncestorsFor(this IAdvancedSessionOperations session, IPage page,
            bool includeStartPage = false)
        {
            try
            {
                var context = new BrickPileContext(HttpContext.Current.Request.RequestContext);
                if (context.Trie == null)
                {
                    throw new NullReferenceException("The RouteData DataToken does not contains the Trie object");
                }
                IEnumerable<TrieNode> nodes = context.Trie.GetAncestors(page.Id, includeStartPage);

                return ((IDocumentSession) session).Load<IPage>(nodes.Select(n => n.PageId)).Values; // TODO handle null values
            }
            catch (ArgumentNullException exception) {}
            return null;
        }

        /// <summary>
        ///     Gets the draft for a specific page, if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static T GetDraftFor<T>(this IAdvancedSessionOperations session, IPage page)
        {
            return ((IDocumentSession) session).Load<T>(page.Id.Replace("/draft", "") + "/draft");
        }

        /// <summary>
        ///     Adds custom funtionality when a page is stored in the <see cref="IDocumentSession" />. Use the action to specify if
        ///     the page should be saved as a draft och published.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <param name="action">The action.</param>
        public static void Store<T>(this IDocumentSession session, T page, StoreAction action) where T : IPage
        {
            session.Store(page);

            HttpContext.Current.Items["storeAction"] = action;

            switch (action)
            {
                case StoreAction.None:
                    break;
                case StoreAction.Save:
                    page.Metadata.Published = default(DateTime?);
                    page.Metadata.IsPublished = false;
                    page.Metadata.Changed = DateTime.UtcNow;
                    page.Metadata.ChangedBy = HttpContext.Current.User.Identity.Name;
                    //if (!page.IsDraft() && session.Advanced.Exists(page.Id))
                    //{
                    //    session.Advanced.Evict(page);
                    //    session.Store(page, page.Id + "/draft");
                    //}
                    break;
                case StoreAction.Publish:
                    page.Metadata.IsPublished = true;
                    page.Metadata.Published = page.Metadata.Published ?? DateTime.UtcNow;
                    page.Metadata.Changed = DateTime.UtcNow;
                    page.Metadata.ChangedBy = HttpContext.Current.User.Identity.Name;
                    //if (page.IsDraft())
                    //{
                    //    session.Advanced.Evict(page);
                    //    session.Store(page, page.Id.Replace("/draft", ""));
                    //}
                    break;
                case StoreAction.UnPublish:
                    page.Metadata.IsPublished = false;
                    page.Metadata.Published = default(DateTime?);
                    page.Metadata.Changed = DateTime.UtcNow;
                    page.Metadata.ChangedBy = HttpContext.Current.User.Identity.Name;
                    break;
            }
        }
    }
}