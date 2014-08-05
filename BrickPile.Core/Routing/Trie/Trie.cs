using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrickPile.Core.Extensions;

namespace BrickPile.Core.Routing.Trie
{
    public sealed class Trie
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the root node.
        /// </summary>
        /// <value>
        ///     The root node.
        /// </value>
        public TrieNode RootNode { get; set; }

        /// <summary>
        ///     Gets the ancestors.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="includeRoot">if set to <c>true</c> [include root].</param>
        /// <returns></returns>
        public IEnumerable<TrieNode> GetAncestors(string pageId, bool includeRoot = false)
        {
            var parent = this.Get(pageId);
            while (parent != null && (includeRoot || parent.PageId != this.RootNode.PageId))
            {
                yield return parent;
                parent = this.RootNode.Flatten(x => x.Children)
                    .Where(node => node.PageId != null)
                    .SingleOrDefault(x => x.PageId == parent.ParentId);
            }
        }

        public IEnumerable<string> GetAncestorIdsFor(string pageId, bool includeRoot = false)
        {
            var ancestors = this.GetAncestors(pageId, includeRoot);

            Func<IEnumerable<TrieNode>, IList<string>> flatten = nodes =>
            {
                var enumerable = nodes as TrieNode[] ?? nodes.ToArray();
                return enumerable.Select(n => n.PageId)
                    .Union(enumerable.SelectMany(n => n.Children).Select(n => n.PageId)).ToList();
            };

            return flatten(ancestors);
        }

        /// <summary>
        ///     Gets the specified page identifier.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns></returns>
        public TrieNode Get(string pageId)
        {
            return this.RootNode == null
                ? null
                : this.RootNode.Flatten(node => node.Children).SingleOrDefault(node => node.PageId == pageId);
        }

        /// <summary>
        ///     Deletes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Delete(TrieNode node)
        {
            var parent = this.RootNode.Flatten(x => x.Children).SingleOrDefault(x => x.Children.Contains(node));
            if (parent != null)
            {
                parent.Children.Remove(node);
            }
        }

        /// <summary>
        ///     Moves to.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="node">The node.</param>
        /// <exception cref="System.Exception">Cannot find parent node, maybe this is the root page?</exception>
        public void MoveTo(TrieNode to, TrieNode node)
        {
            var parent = this.RootNode.Flatten(x => x.Children).SingleOrDefault(x => x.Children.Contains(node));

            if (parent == null)
            {
                throw new Exception("Cannot find parent node, maybe this is the root page?");
            }

            parent.Children.Remove(node);

            to.Children.Add(node);
            node.ParentId = to.PageId;

            this.UpdateUrls(to);
        }

        /// <summary>
        ///     Updates the urls.
        /// </summary>
        /// <param name="node">The node.</param>
        private void UpdateUrls(TrieNode node)
        {
            foreach (var child in node.Children)
            {
                var slug = child.Url != null ? child.Url.Split('/').Last() : "";

                if (!string.IsNullOrEmpty(child.ParentId))
                {
                    if (child.ParentId == this.RootNode.PageId)
                    {
                        child.Url = slug;
                    }
                    else
                    {
                        var parent = this.Get(child.ParentId);
                        child.Url = slug.Insert(0, VirtualPathUtility.AppendTrailingSlash(parent.Url));
                    }
                }

                this.UpdateUrls(child);
            }
        }
    }
}
