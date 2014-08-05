using System;
using BrickPile.Core.Routing.Trie;

namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Defines the methods that are required for an <see cref="IRouteResolver" />.
    /// </summary>
    internal interface IRouteResolver
    {
        /// <summary>
        /// Resolves the route based on the incoming <see cref="string" /> url.
        /// </summary>
        /// <param name="trie">The trie.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        Tuple<TrieNode, string> ResolveRoute(Trie.Trie trie, string virtualPath);
    }
}