using System.Web.Mvc;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing;
using Raven.Client;
using Raven.Client.Documents;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Specifies that access to a page is restricted to users who meet the authorization requirement.
    /// </summary>
    public class AuthorizeContentAttribute : AuthorizeAttribute
    {
        private readonly IDocumentStore documentStore;

        /// <summary>
        /// Gets or sets the editor action.
        /// </summary>
        /// <value>
        /// The editor action.
        /// </value>
        public EditorAction EditorAction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeContentAttribute"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public AuthorizeContentAttribute(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
            this.EditorAction = EditorAction.None;
        }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
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

            using (var session = documentStore.OpenSession())
            {
                if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated &&
                    session.Advanced.Exists(currentPage.Id + "/draft"))
                {
                    var editAction = (EditorAction)filterContext.Controller.TempData["EditorAction"];
                    // replace the current page with the draft if the user is logged on and we have a draft saved
                    if (editAction != EditorAction.Preview) return;
                    filterContext.RouteData.Values[DefaultRoute.CurrentPageKey] =
                        session.Load<IPage>(currentPage.Id + "/draft");
                    filterContext.Controller.TempData["EditorAction"] = EditorAction.Preview;
                }
            }
        }
    }
}