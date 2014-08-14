using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Mvc;
using BrickPile.Domain;
using Raven.Client;

namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Represent the default an ASP.NET route for BrickPile pages.
    /// </summary>
    internal class DefaultRoute : RouteBase
    {
        public const string ControllerKey = "controller";
        public const string CurrentPageKey = "currentPage";
        public const string ActionKey = "action";
        public const string DefaultAction = "index";

        /// <summary>
        ///     Gets the virtual path resolver.
        /// </summary>
        /// <value>
        ///     The virtual path resolver.
        /// </value>
        protected IVirtualPathResolver VirtualPathResolver { get; private set; }

        /// <summary>
        ///     Gets the route resolver.
        /// </summary>
        /// <value>
        ///     The route resolver.
        /// </value>
        protected IRouteResolver RouteResolver { get; private set; }

        /// <summary>
        ///     Gets the document store.
        /// </summary>
        /// <value>
        ///     The document store.
        /// </value>
        protected IDocumentStore DocumentStore { get; private set; }

        /// <summary>
        ///     Gets the controller mapper.
        /// </summary>
        /// <value>
        ///     The controller mapper.
        /// </value>
        protected IControllerMapper ControllerMapper { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultRoute" /> class.
        /// </summary>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <param name="routeResolver">The route resolver.</param>
        /// <param name="documentStore">The document store.</param>
        /// <param name="controllerMapper">The controller mapper.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     virtualPathResolver
        ///     or
        ///     routeResolver
        ///     or
        ///     documentStore
        ///     or
        ///     controllerMapper
        /// </exception>
        public DefaultRoute(IVirtualPathResolver virtualPathResolver, IRouteResolver routeResolver,
            IDocumentStore documentStore, IControllerMapper controllerMapper)
        {
            if (virtualPathResolver == null)
            {
                throw new ArgumentNullException("virtualPathResolver");
            }

            if (routeResolver == null)
            {
                throw new ArgumentNullException("routeResolver");
            }

            if (documentStore == null)
            {
                throw new ArgumentNullException("documentStore");
            }

            if (controllerMapper == null)
            {
                throw new ArgumentNullException("controllerMapper");
            }

            this.VirtualPathResolver = virtualPathResolver;
            this.RouteResolver = routeResolver;
            this.DocumentStore = documentStore;
            this.ControllerMapper = controllerMapper;
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
            // Abort and proceed to other routes in the route table if path contains api or ui
            string[] segments = httpContext.Request.Path.Split(new[] {'/'});
            if (segments.Any(segment => segment.Equals("api", StringComparison.OrdinalIgnoreCase) ||
                                        segment.Equals("ui", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            ResolveResult result = this.RouteResolver.Resolve(httpContext.Request.Path);

            if (result == null)
            {
                return null;
            }

            IPage currentPage;
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                currentPage = session.Load<IPage>(result.TrieNode.PageId);
            }

            string controllerName = this.ResolveControllerName(currentPage);

            if (!this.ControllerMapper.ControllerHasAction(controllerName, result.Action))
            {
                return null;
            }

            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.Values[ControllerKey] = controllerName;
            routeData.Values[ActionKey] = result.Action;
            routeData.Values[CurrentPageKey] = currentPage;

            return routeData;
        }

        /// <summary>
        ///     Resolves the name of the controller.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Missing ContentType attribute</exception>
        protected string ResolveControllerName(IPage currentPage)
        {
            var contentTypeAttribute = currentPage.GetType().GetAttribute<ContentTypeAttribute>();

            if (contentTypeAttribute == null)
            {
                throw new NullReferenceException("Missing ContentType attribute");
            }

            return contentTypeAttribute.ControllerType == null
                ? currentPage.GetType().Name
                : this.ControllerMapper.GetControllerName(contentTypeAttribute.ControllerType);
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
            IPage model = values[CurrentPageKey] as IPage ?? requestContext.RouteData.Values[CurrentPageKey] as IPage;

            if (model == null)
            {
                return null;
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.Resolve(model, values));

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