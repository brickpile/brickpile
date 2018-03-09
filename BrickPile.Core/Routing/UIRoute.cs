using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Associates a route with the BrickPile UI area.
    /// </summary>
    internal class UiRoute : DefaultRoute, IRouteWithArea
    {
        private const string ControllerName = "pages";

        /// <summary>
        ///     Initializes a new instance of the <see cref="UiRoute" /> class.
        /// </summary>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <param name="routeResolver">The route resolver.</param>
        /// <param name="documentStore">The document store.</param>
        /// <param name="controllerMapper">The controller mapper.</param>
        public UiRoute(IVirtualPathResolver virtualPathResolver, IRouteResolver routeResolver,
            IDocumentStore documentStore,
            IControllerMapper controllerMapper)
            : base(virtualPathResolver, routeResolver, documentStore, controllerMapper) {}

        /// <summary>
        ///     Gets the name of the area to associate the route with.
        /// </summary>
        /// <returns>The name of the area to associate the route with.</returns>
        public string Area
        {
            get { return "UI"; }
        }

        /// <summary>
        ///     When overridden in a derived class, returns route information about the request.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        ///     An object that contains the values from the route definition if the route matches the current request, or null if
        ///     the route does not match the request.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string[] segments = httpContext.Request.Path.Split(new[] {'/'});
            if (!segments.Any(segment => segment.Equals("ui", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            ResolveResult result = RouteResolver.Resolve(httpContext.Request.Path.Replace("/ui/pages", ""));

            if (result == null)
            {
                return null;
            }

            IPage currentPage;
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                currentPage = session.Load<IPage>(result.TrieNode.PageId);
            }

            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.Values[ControllerKey] = ControllerName;
            routeData.Values[ActionKey] = result.Action;
            routeData.Values[CurrentPageKey] = currentPage;

            return routeData;
        }

        /// <summary>
        ///     When overridden in a derived class, checks whether the route matches the specified values, and if so, generates a
        ///     URL and retrieves information about the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        ///     An object that contains the generated URL and information about the route, or null if the route does not match
        ///     <paramref name="values" />.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var model = values[CurrentPageKey] as IPage;

            if (model == null)
            {
                return null;
            }

            var vpd = new VirtualPathData(this, VirtualPathResolver.Resolve(model, values));

            vpd.VirtualPath = string.Format("/ui/pages".TrimStart(new[] {'/'}) + "/{0}", vpd.VirtualPath);

            string queryParams = String.Empty;
            // add query string parameters
            foreach (var kvp in values)
            {
                if (kvp.Key.Equals(CurrentPageKey) || kvp.Key.Equals(ControllerKey) || kvp.Key.Equals(ActionKey))
                {
                    continue;
                }
                queryParams = queryParams.AddQueryParam(kvp.Key, kvp.Value.ToString());
            }
            vpd.VirtualPath += queryParams;
            return vpd;
        }
    }
}