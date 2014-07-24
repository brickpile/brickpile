using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using StructureMap;

namespace BrickPile.Core.Extensions {
    public static class DocumentSessionExtensions {

        /// <summary>
        /// Get the start page
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IPage GetStartPage(this ISyncAdvancedSessionOperation session) {
            var structureInfo = ObjectFactory.GetInstance<IStructureInfo>();
            return ((DocumentSession) session).Load<Page>(structureInfo.RootNode.PageId);
        }

        /// <summary>
        /// Gets the children for.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetChildrenFor<T>(this ISyncAdvancedSessionOperation session, IPage page)
            where T : IPage {
            var structureInfo = ObjectFactory.GetInstance<IStructureInfo>();

            var nodes = structureInfo.RootNode.Flatten(n => n.Children);

            var node = nodes.SingleOrDefault(n => n.PageId == page.Id.Replace("/draft",""));

            return node != null
                ? ((DocumentSession)session).Load<T>(node.Children.Select(n => n.PageId).ToArray()).OrderBy(p => p.Metadata.SortOrder)
                : Enumerable.Empty<T>();
        }

        /// <summary>
        /// Gets the children for.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        internal static IEnumerable<T2> GetChildrenFor<T1, T2>(this ISyncAdvancedSessionOperation session, IPage page)
            where T1 : AbstractTransformerCreationTask, new()
            where T2 : Page {
            var structureInfo = ObjectFactory.GetInstance<IStructureInfo>();

            var nodes = structureInfo.RootNode.Flatten(n => n.Children);

            var node = nodes.SingleOrDefault(n => n.PageId == page.Id.Replace("/draft",""));

            return node != null
                ? ((DocumentSession) session).Load<T1, T2>(node.Children.Select(n => n.PageId).ToArray())
                : Enumerable.Empty<T2>();
        }

        /// <summary>
        /// Filters for visitor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> pages) where T : IPage
        {
            var p = pages.ToList();

            for (var i = 0; i < p.Count; i++)
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated && !p[i].Metadata.IsPublished && !p[i].Metadata.DisplayInMenu)
                {
                    p.RemoveAt(i);
                    i--;
                }
            }

            return p.OrderBy(x => x.Metadata.SortOrder);
        }

        /// <summary>
        /// Gets the parent for.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static IPage GetParentFor(this ISyncAdvancedSessionOperation session, IPage page) {
            return page.Parent == null ? null : ((DocumentSession)session).Load<IPage>(page.Parent.Id);
        }

        /// <summary>
        /// Gets the ancestors for.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <param name="includeStartPage">if set to <c>true</c> [include start page].</param>
        /// <returns></returns>
        public static IEnumerable<IPage> GetAncestorsFor(this ISyncAdvancedSessionOperation session, IPage page,
            bool includeStartPage = false) {
            var structureInfo = ObjectFactory.GetInstance<IStructureInfo>();

            var nodes = structureInfo.GetAncestors(page.Id, includeStartPage);

            return ((DocumentSession) session).Load<IPage>(nodes.Select(n => n.PageId));
        }

        /// <summary>
        /// Gets the navigation context for a page including all ancestors and each ancestors children
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <param name="includeStartPage">if set to <c>true</c> [include start page].</param>
        /// <returns></returns>
        public static IEnumerable<IPage> GetNavigationContextFor(this ISyncAdvancedSessionOperation session, IPage page,
            bool includeStartPage = false) {
            var structureInfo = ObjectFactory.GetInstance<IStructureInfo>();

            var ancestors = structureInfo.GetAncestors(page.Id.Replace("/draft", ""), includeStartPage);

            Func<IEnumerable<StructureInfo.Node>, IEnumerable<string>> flatten = null;
            flatten = nodes => nodes.Select(n => n.PageId)
                .Union(nodes.SelectMany(n => n.Children).Select(n => n.PageId));

            return
                ((DocumentSession) session).Load<IPage>(flatten(ancestors))
                    .OrderBy(p => p.Metadata.SortOrder);
        }

        /// <summary>
        /// Stores the specified page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="page">The page.</param>
        /// <param name="action">The action.</param>
        public static void Store<T>(this IDocumentSession session, T page, StoreAction action) where T : IPage {

            session.Store(page);

            HttpContext.Current.Items["storeAction"] = action;

            switch (action) {
                    case StoreAction.None:
                    break;
                case StoreAction.Save:
                    page.Metadata.Published = default(DateTime?);
                    page.Metadata.IsPublished = false;
                    page.Metadata.Changed = DateTime.UtcNow;
                    page.Metadata.ChangedBy = HttpContext.Current.User.Identity.Name;
                    if (!page.IsDraft() && session.Advanced.DocumentStore.Exists(page.Id)) {
                        session.Advanced.Evict(page);
                        session.Store(page, page.Id + "/draft");
                    }
                    break;
                case StoreAction.Publish:
                    page.Metadata.IsPublished = true;
                    page.Metadata.Published = DateTime.UtcNow;
                    page.Metadata.Changed = DateTime.UtcNow;
                    page.Metadata.ChangedBy = HttpContext.Current.User.Identity.Name;
                    if (page.IsDraft()) {
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

        public static void Store<T>(this IDocumentSession session, T page, T parent, StoreAction action) where T : IPage { }
    }
}