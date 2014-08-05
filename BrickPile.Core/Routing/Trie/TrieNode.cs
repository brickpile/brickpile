using System.Collections.Generic;

namespace BrickPile.Core.Routing.Trie
{
    public sealed class TrieNode
    {
        /// <summary>
        ///     Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        ///     The parent identifier.
        /// </value>
        public string ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the page identifier.
        /// </summary>
        /// <value>
        ///     The page identifier.
        /// </value>
        public string PageId { get; set; }

        /// <summary>
        ///     Gets or sets the URL.
        /// </summary>
        /// <value>
        ///     The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the children.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public List<TrieNode> Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        public TrieNode()
        {
            this.Children = new List<TrieNode>();
        }
    }
}
