using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Document;

namespace BrickPile.Core.Extensions {
    public static class DocumentSessionExtensions {

        public static IPage GetStartPage(this ISyncAdvancedSessionOperation session) {
            var structureInfo = HttpContext.Current.Request.RequestContext.RouteData.GetStructureInfo();
            if (structureInfo == null) {
                throw new NullReferenceException("StructureInfo");
            }
            return ((DocumentSession) session).Load<Page>(structureInfo.RootNode.PageId);
        }

        public static IEnumerable<IPage> GetChildrenFor(this ISyncAdvancedSessionOperation session, IPage page) {
            var structureInfo = HttpContext.Current.Request.RequestContext.RouteData.GetStructureInfo();
            if (structureInfo == null)
            {
                throw new NullReferenceException("StructureInfo");
            }

            var nodes = structureInfo.RootNode.Flatten(n => n.Children);

            var node = nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(page.Id));

            return node != null
                ? ((DocumentSession)session).Load<IPage>(node.Children.Select(n => n.PageId).ToArray()).OrderBy(p => p.Metadata.SortOrder)
                : Enumerable.Empty<IPage>();
        }

        public static T GetParentFor<T>(this ISyncAdvancedSessionOperation session, IPage page) where T : IPage {
            return page.Parent == null ? default(T) : ((DocumentSession) session).Load<T>(page.Parent.Id);
        }

        public static IEnumerable<IPage> GetAncestorsFor(this ISyncAdvancedSessionOperation session, IPage page,
            bool includeStartPage = false) {
            var structureInfo = HttpContext.Current.Request.RequestContext.RouteData.GetStructureInfo();
            if (structureInfo == null)
            {
                throw new NullReferenceException("StructureInfo");
            }
            var nodes = structureInfo.GetAncestors(page.Id, includeStartPage);

            return ((DocumentSession) session).Load<IPage>(nodes.Select(n => n.PageId));
        }

        public static T GetDraftFor<T>(this ISyncAdvancedSessionOperation session, IPage page) {           
            return ((DocumentSession) session).Load<T>(page.Id.Replace("/draft","") + "/draft");
        }

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
    }
}