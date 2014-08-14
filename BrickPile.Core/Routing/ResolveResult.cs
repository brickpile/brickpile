using BrickPile.Core.Routing.Trie;

namespace BrickPile.Core.Routing
{
    public class ResolveResult
    {
        /// <summary>
        ///     Gets or sets the trie node.
        /// </summary>
        /// <value>
        ///     The trie node.
        /// </value>
        public TrieNode TrieNode { get; set; }

        /// <summary>
        ///     Gets or sets the action.
        /// </summary>
        /// <value>
        ///     The action.
        /// </value>
        public string Action { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResolveResult" /> class.
        /// </summary>
        /// <param name="trieNode">The trie node.</param>
        /// <param name="action">The action.</param>
        public ResolveResult(TrieNode trieNode, string action)
        {
            this.TrieNode = trieNode;
            this.Action = action;
        }
    }
}