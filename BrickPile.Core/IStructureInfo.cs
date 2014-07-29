using System.Collections.Generic;

namespace BrickPile.Core {
    public interface IStructureInfo {
        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        StructureInfo.Node RootNode { get; set; }

        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="includeRoot">if set to <c>true</c> [include root].</param>
        /// <returns></returns>
        IEnumerable<StructureInfo.Node> GetAncestors(string pageId, bool includeRoot = false);
    }
}