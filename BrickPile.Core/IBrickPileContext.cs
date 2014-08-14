using System.Web.Routing;
using BrickPile.Core.Routing.Trie;

namespace BrickPile.Core
{
    public interface IBrickPileContext
    {
        /// <summary>
        ///     Gets or sets the request context.
        /// </summary>
        /// <value>
        ///     The request context.
        /// </value>
        RequestContext RequestContext { get; set; }

        /// <summary>
        ///     Gets or sets the trie.
        /// </summary>
        /// <value>
        ///     The trie.
        /// </value>
        Trie Trie { get; set; }

        /// <summary>
        ///     Gets or sets the navigation context.
        /// </summary>
        /// <value>
        ///     The navigation context.
        /// </value>
        INavigationContext NavigationContext { get; set; }
    }
}