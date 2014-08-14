using System;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing.Trie;

namespace BrickPile.Core
{
    public sealed class BrickPileContext : IBrickPileContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BrickPileContext" /> class.
        /// </summary>
        public BrickPileContext() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="BrickPileContext" /> class.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <exception cref="System.ArgumentNullException">requestContext</exception>
        public BrickPileContext(RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            this.RequestContext = requestContext;
            this.Trie = requestContext.RouteData.GetTrie();
            this.NavigationContext = new NavigationContext(requestContext);
        }

        /// <summary>
        ///     Gets or sets the request context.
        /// </summary>
        /// <value>
        ///     The request context.
        /// </value>
        public RequestContext RequestContext { get; set; }

        /// <summary>
        ///     Gets or sets the trie.
        /// </summary>
        /// <value>
        ///     The trie.
        /// </value>
        public Trie Trie { get; set; }

        /// <summary>
        ///     Gets or sets the navigation context.
        /// </summary>
        /// <value>
        ///     The navigation context.
        /// </value>
        public INavigationContext NavigationContext { get; set; }
    }
}