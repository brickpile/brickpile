﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrickPile.Core.Extensions;

namespace BrickPile.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class StructureInfo : IStructureInfo {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        public Node RootNode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class Node
        {

            /// <summary>
            /// Gets or sets the parent identifier.
            /// </summary>
            /// <value>
            /// The parent identifier.
            /// </value>
            public string ParentId { get; set; }

            /// <summary>
            /// Gets or sets the page identifier.
            /// </summary>
            /// <value>
            /// The page identifier.
            /// </value>
            public string PageId { get; set; }

            /// <summary>
            /// Gets or sets the URL.
            /// </summary>
            /// <value>
            /// The URL.
            /// </value>
            public string Url { get; set; }

            /// <summary>
            /// Gets or sets the children.
            /// </summary>
            /// <value>
            /// The children.
            /// </value>
            public List<Node> Children { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            public Node()
            {
                Children = new List<Node>();
            }
        }

        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="includeRoot">if set to <c>true</c> [include root].</param>
        /// <returns></returns>
        public IEnumerable<Node> GetAncestors(string pageId, bool includeRoot = false)
        {
            var parent = Get(pageId);
            while (parent != null && (includeRoot || parent.PageId != RootNode.PageId))
            {
                yield return parent;                
                parent = RootNode.Flatten(x => x.Children)
                    .Where(node => node.PageId != null)
                    .SingleOrDefault(x => x.PageId == parent.ParentId);
            }
        }

        /// <summary>
        /// Gets the specified page identifier.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns></returns>        
        public Node Get(string pageId)
        {
            return RootNode == null ? null : RootNode.Flatten(node => node.Children).SingleOrDefault(node => node.PageId == pageId);
        }

        /// <summary>
        /// Deletes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Delete(Node node)
        {
            var parent = RootNode.Flatten(x => x.Children).SingleOrDefault(x => x.Children.Contains(node));
            if (parent != null) { 
                parent.Children.Remove(node);
            }
        }

        /// <summary>
        /// Moves to.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="node">The node.</param>
        /// <exception cref="System.Exception">Cannot find parent node, maybe this is the root page?</exception>
        public void MoveTo(Node to, Node node)
        {
            var parent = RootNode.Flatten(x => x.Children).SingleOrDefault(x => x.Children.Contains(node));

            if (parent == null)
            {
                throw new System.Exception("Cannot find parent node, maybe this is the root page?");
            }

            parent.Children.Remove(node);

            to.Children.Add(node);
            node.ParentId = to.PageId;

            UpdateUrls(to);
        }

        /// <summary>
        /// Updates the urls.
        /// </summary>
        /// <param name="node">The node.</param>
        private void UpdateUrls(Node node)
        {
            foreach (var child in node.Children)
            {
                var slug = child.Url != null ? child.Url.Split('/').Last() : "";

                if (!string.IsNullOrEmpty(child.ParentId))
                {
                    if (child.ParentId == RootNode.PageId)
                    {
                        child.Url = slug;
                    }
                    else
                    {
                        var parent = Get(child.ParentId);
                        child.Url = slug.Insert(0, VirtualPathUtility.AppendTrailingSlash(parent.Url));
                    }
                }

                UpdateUrls(child);
            }
        }
    }
}