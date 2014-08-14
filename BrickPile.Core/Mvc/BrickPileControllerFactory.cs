using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing;
using BrickPile.Core.Routing.Trie;
using Raven.Client;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Represents the controller factory that is registered by BrickPile.
    /// </summary>
    internal sealed class BrickPileControllerFactory : DefaultControllerFactory
    {
        private static bool hasConfiguration;
        private readonly IBrickPileContextFactory contextFactory;
        private readonly IDocumentStore documentStore;
        private readonly IRouteResolverTrie routeResolverTrie;
        private readonly INavigationContextFactory navigationContextFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BrickPileControllerFactory" /> class.
        /// </summary>
        public BrickPileControllerFactory(IDocumentStore documentStore, IBrickPileContextFactory contextFactory,
            IRouteResolverTrie routeResolverTrie, INavigationContextFactory navigationContextFactory)
        {
            this.documentStore = documentStore;
            this.contextFactory = contextFactory;
            this.routeResolverTrie = routeResolverTrie;
            this.navigationContextFactory = navigationContextFactory;
        }

        /// <summary>
        ///     Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>
        ///     The controller.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestContext" /> parameter is null.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="controllerName" /> parameter is null or empty.</exception>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (this.HasConfiguration())
            {
                this.CreateContext(requestContext);
                return base.CreateController(requestContext, controllerName);
            }
            requestContext.RouteData.DataTokens["area"] = "ui";
            requestContext.RouteData.Values["action"] = "index";
            requestContext.RouteData.Values["controller"] = "setup";
            return base.CreateController(requestContext, "setup");
        }

        /// <summary>
        ///     Creates the brick pile context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private void CreateContext(RequestContext requestContext)
        {
            var currentPage = requestContext.RouteData.GetCurrentPage<IPage>();
            if (currentPage == null)
                return;

            BrickPileContext context = this.contextFactory.Create(requestContext);

            requestContext.RouteData.ApplyTrie(context.Trie);
            requestContext.RouteData.ApplyCurrentContext(context);
            requestContext.RouteData.ApplyPages(context.NavigationContext.OpenPages);
        }

        /// <summary>
        ///     Determines whether this instance has configuration.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance has configuration; otherwise, <c>false</c>.
        /// </returns>
        private bool HasConfiguration()
        {
            if (!hasConfiguration)
            {
                hasConfiguration = this.documentStore.Exists("brickpile/configuration");
            }
            return hasConfiguration;
        }
    }
}