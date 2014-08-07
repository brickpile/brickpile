using System;
using System.Linq;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing.Trie;

namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Represents the default <see cref="RouteResolver" />.
    /// </summary>
    internal class RouteResolver : IRouteResolver
    {
        public Tuple<TrieNode, string> ResolveRoute(Trie.Trie trie, string virtualPath)
        {
            // Set the default action to index
            var action = DefaultRoute.DefaultAction;

            if (trie == null || trie.RootNode == null) return null;

            TrieNode currentNode;

            var segments = virtualPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            // The requested url is for the start page with no action
            // so just return the start page
            if (!segments.Any())
            {
                currentNode = trie.RootNode;
            }
            else
            {
                var nodes = trie.RootNode.Flatten(node => node.Children).ToList();

                // The normal behaviour is to load the page based on the incoming url
                currentNode = nodes.SingleOrDefault(n => n.Url == string.Join("/", segments));

                // Try to find the node without the last segment of the url and set the last segment as action
                if (currentNode == null)
                {
                    action = segments.Last();
                    virtualPath = string.Join("/", segments, 0, (segments.Length - 1));
                    currentNode =
                        nodes.SingleOrDefault(n => n.Url == (string.IsNullOrEmpty(virtualPath) ? null : virtualPath));
                }
            }

            return currentNode == null ? null : new Tuple<TrieNode, string>(currentNode, action);
        }
    }
}