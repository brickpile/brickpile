using System.Web.Mvc;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing;
using Raven.Client;

namespace BrickPile.Core.Mvc
{
    public class AuthorizeContentAttribute : AuthorizeAttribute
    {
        private readonly IDocumentStore documentStore;

        public EditorAction EditorAction { get; set; }

        public AuthorizeContentAttribute(IDocumentStore documentStore) {
            this.documentStore = documentStore;
            this.EditorAction = EditorAction.None;
        }

        public override void OnAuthorization(AuthorizationContext filterContext) {
            base.OnAuthorization(filterContext);

            var currentPage = filterContext.RouteData.GetCurrentPage<IPage>();

            if (currentPage == null)
            {
                return;
            }

            if ((!currentPage.Metadata.IsPublished || currentPage.Metadata.IsDeleted) &&
                !filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            if (!filterContext.Controller.TempData.ContainsKey("EditorAction")) return;

            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && documentStore.Exists(currentPage.Id + "/draft"))
            {
                var editAction = (EditorAction)filterContext.Controller.TempData["EditorAction"];
                // replace the current page with the draft if the user is logged on and we have a draft saved
                if (editAction != EditorAction.Preview) return;
                filterContext.RouteData.Values[PageRoute.CurrentPageKey] =
                    this.documentStore.OpenSession().Load<IPage>(currentPage.Id + "/draft");
                filterContext.Controller.TempData["EditorAction"] = EditorAction.Preview;
            }
        }
    }
}