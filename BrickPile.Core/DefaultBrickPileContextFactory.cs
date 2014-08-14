using System.Web.Routing;
using BrickPile.Core.Routing;
using BrickPile.Core.Routing.Trie;

namespace BrickPile.Core
{
    public class DefaultBrickPileContextFactory : IBrickPileContextFactory
    {
        private readonly IRouteResolverTrie routeResolverTrie;
        private readonly INavigationContextFactory navigationContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBrickPileContextFactory"/> class.
        /// </summary>
        /// <param name="routeResolverTrie">The route resolver trie.</param>
        /// <param name="navigationContextFactory">The navigation context factory.</param>
        public DefaultBrickPileContextFactory(IRouteResolverTrie routeResolverTrie,
            INavigationContextFactory navigationContextFactory)
        {
            this.routeResolverTrie = routeResolverTrie;
            this.navigationContextFactory = navigationContextFactory;
        }

        /// <summary>
        ///     Creates the specified current trie.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public BrickPileContext Create(RequestContext requestContext)
        {
            var context = new BrickPileContext
            {
                RequestContext = requestContext,
                Trie = this.routeResolverTrie.LoadTrie(),
                NavigationContext = this.navigationContextFactory.Create()
            };

            return context;
        }
    }
}