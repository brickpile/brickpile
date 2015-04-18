using System;
using System.Linq;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing.Trie;
using Raven.Client;

namespace BrickPile.Core.Routing
{
    public class DefaultNavigationContextFactory : INavigationContextFactory
    {
        private readonly Func<RequestContext> requestContext;
        private readonly IRouteResolverTrie routeResolverTrie;
        private readonly IDocumentStore documentStore;

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>
        /// The request context.
        /// </value>
        protected RequestContext RequestContext
        {
            get { return this.requestContext.Invoke(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNavigationContextFactory" /> class.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="routeResolverTrie">The route resolver trie.</param>
        /// <param name="documentStore">The document store.</param>
        public DefaultNavigationContextFactory(Func<RequestContext> requestContext, IRouteResolverTrie routeResolverTrie, IDocumentStore documentStore)
        {
            this.requestContext = requestContext;
            this.routeResolverTrie = routeResolverTrie;
            this.documentStore = documentStore;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public NavigationContext Create()
        {
            if (this.RequestContext.HttpContext.Items.Contains("brickpile:navigationContext"))
            {
                return this.RequestContext.HttpContext.Items["brickpile:navigationContext"] as NavigationContext;
            }

            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                IPage[] pages = session.Load<IPage>(
                    this.routeResolverTrie.LoadTrie().GetAncestorIdsFor(
                        this.RequestContext.RouteData.GetCurrentPage<IPage>().Id,
                        true)).OrderBy(x => x.Metadata.SortOrder).ToArray();

                var navigationContext = new NavigationContext
                {
                    StartPage = pages.SingleOrDefault(x => x.Parent == null),
                    CurrentPage = this.RequestContext.RouteData.GetCurrentPage<IPage>(),
                    OpenPages = pages,
                    RequestContext = this.RequestContext
                };

                this.RequestContext.HttpContext.Items["brickpile:navigationContext"] = navigationContext;

                return navigationContext;
            }            
        }
    }
}