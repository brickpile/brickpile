using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing.Trie;
using BrickPile.Domain;
using Raven.Client;

namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Represent an ASP.NET custom route for BrickPile pages.
    /// </summary>
    internal class PageRoute : RouteBase
    {
        public const string ControllerKey = "controller";

        /// <summary>
        ///     Gets the virtual path resolver.
        /// </summary>
        /// <value>
        ///     The virtual path resolver.
        /// </value>
        protected VirtualPathResolver VirtualPathResolver { get; private set; }

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
        protected Func<IDocumentStore> DocumentStore { get; private set; }

        /// <summary>
        ///     Gets the controller mapper.
        /// </summary>
        /// <value>
        ///     The controller mapper.
        /// </value>
        protected IControllerMapper ControllerMapper { get; private set; }


        /// <summary>
        /// Gets or sets the trie.
        /// </summary>
        /// <value>
        /// The trie.
        /// </value>
        protected Trie.Trie Trie { get; set; }

        /// <summary>
        ///     Gets the current page key.
        /// </summary>
        /// <value>
        ///     The current page key.
        /// </value>
        public static string CurrentPageKey
        {
            get { return "currentPage"; }
        }

        /// <summary>
        ///     Gets the action key.
        /// </summary>
        /// <value>
        ///     The action key.
        /// </value>
        public static string ActionKey
        {
            get { return "action"; }
        }

        /// <summary>
        ///     Gets the default action.
        /// </summary>
        /// <value>
        ///     The default action.
        /// </value>
        public static string DefaultAction
        {
            get { return "index"; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PageRoute" /> class.
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
        public PageRoute(VirtualPathResolver virtualPathResolver, IRouteResolver routeResolver,
            Func<IDocumentStore> documentStore, IControllerMapper controllerMapper)
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
            var segments = httpContext.Request.Path.Split(new[] {'/'});
            if (segments.Any(segment => segment.Equals("api", StringComparison.OrdinalIgnoreCase) ||
                                        segment.Equals("ui", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            using (var session = this.DocumentStore.Invoke().OpenSession())
            {
                this.Trie = session.Load<Trie.Trie>(DefaultBrickPileBootstrapper.TrieId);
            }

            var nodeAndAction = this.RouteResolver.ResolveRoute(this.Trie,
                httpContext.Request.Path);

            if (nodeAndAction == null)
            {
                return null;
            }

            var navigationContext = this.PrepareNavigationContext(
                httpContext.Request.RequestContext,
                nodeAndAction);

            var controllerName = this.ResolveControllerName(navigationContext.CurrentPage);

            if (!this.ControllerMapper.ControllerHasAction(controllerName, nodeAndAction.Item2))
            {
                return null;
            }

            var routeData = this.PrepareRouteData(
                this.Trie,
                navigationContext,
                controllerName,
                nodeAndAction.Item2);

            return routeData;
        }

        /// <summary>
        ///     Prepares the navigation context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="nodeAndAction">The node and action.</param>
        /// <returns></returns>
        protected NavigationContext PrepareNavigationContext(RequestContext requestContext,
            Tuple<TrieNode, string> nodeAndAction)
        {
            using (var session = this.DocumentStore.Invoke().OpenSession())
            {
                var pages =
                    session.Load<IPage>(this.Trie.GetAncestorIdsFor(nodeAndAction.Item1.PageId, true));
                return new NavigationContext(requestContext)
                {
                    CurrentContext = pages,
                    CurrentPage = pages.SingleOrDefault(page => page.Id == nodeAndAction.Item1.PageId)
                };
            }
        }

        /// <summary>
        /// Prepares the route data.
        /// </summary>
        /// <param name="trie">The trie.</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        protected RouteData PrepareRouteData(Trie.Trie trie, NavigationContext navigationContext,
            string controllerName, string action)
        {
            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.ApplyTrie(trie);
            routeData.ApplyCurrentContext(navigationContext.CurrentContext);
            routeData.Values[ControllerKey] = controllerName;
            routeData.Values[ActionKey] = action;
            routeData.Values[CurrentPageKey] = navigationContext.CurrentPage;
            return routeData;
        }

        /// <summary>
        ///     Resolves the name of the controller.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Missing ContentType attribute</exception>
        protected virtual string ResolveControllerName(IPage currentPage)
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
            var model = values[CurrentPageKey] as IPage ?? requestContext.RouteData.Values[CurrentPageKey] as IPage;

            if (model == null)
            {
                return null;
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.ResolveVirtualPath(model, values));

            var queryParams = String.Empty;
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