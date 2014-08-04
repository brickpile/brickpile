using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Document;

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
        public static IPage GetStartPage(this ISyncAdvancedSessionOperation session)
        {
            var structureInfo = HttpContext.Current.Request.RequestContext.RouteData.GetStructureInfo();
            if (structureInfo == null)
            {
                throw new NullReferenceException("The RouteData DataToken does not contains the StructureInfo object.");
            }
            return ((DocumentSession) session).Load<Page>(structureInfo.RootNode.PageId);
        }

        /// <summary>
        ///     Gets the children for specific a specific page
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">StructureInfo</exception>
        public static IEnumerable<IPage> GetChildrenFor(this ISyncAdvancedSessionOperation session, IPage page)
        {
            var structureInfo = HttpContext.Current.Request.RequestContext.RouteData.GetStructureInfo();
            if (structureInfo == null)
            {
                throw new NullReferenceException("The RouteData DataToken does not contains the StructureInfo object");
            }

            var nodes = structureInfo.RootNode.Flatten(n => n.Children);

            var node = nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(page.Id));

            return node != null
                ? ((DocumentSession) session).Load<IPage>(node.Children.Select(n => n.PageId).ToArray())
                    .OrderBy(p => p.Metadata.SortOrder)
                : Enumerable.Empty<IPage>();
        }

        /// <summary>
        ///     Gets the parent for a specific page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static T GetParentFor<T>(this ISyncAdvancedSessionOperation session, IPage page) where T : IPage
        {
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
        public static IEnumerable<IPage> GetAncestorsFor(this ISyncAdvancedSessionOperation session, IPage page,
            bool includeStartPage = false)
        {
            var structureInfo = HttpContext.Current.Request.RequestContext.RouteData.GetStructureInfo();
            if (structureInfo == null)
            {
                throw new NullReferenceException("The RouteData DataToken does not contains the StructureInfo object");
            }
            var nodes = structureInfo.GetAncestors(page.Id, includeStartPage);

            return ((DocumentSession) session).Load<IPage>(nodes.Select(n => n.PageId));
        }

        /// <summary>
        ///     Gets the draft for a specific page, if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static T GetDraftFor<T>(this ISyncAdvancedSessionOperation session, IPage page)
        {
            return ((DocumentSession) session).Load<T>(page.Id.Replace("/draft", "") + "/draft");
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
                    if (!page.IsDraft() && session.Advanced.DocumentStore.Exists(page.Id))
                    {
                        session.Advanced.Evict(page);
                        session.Store(page, page.Id + "/draft");
                    }
                    break;
                case StoreAction.Publish:
                    page.Metadata.IsPublished = true;
                    page.Metadata.Published = DateTime.UtcNow;
                    page.Metadata.Changed = DateTime.UtcNow;
                    page.Metadata.ChangedBy = HttpContext.Current.User.Identity.Name;
                    if (page.IsDraft())
                    {
                        session.Advanced.Evict(page);
                        session.Store(page, page.Id.Replace("/draft", ""));
                    }
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