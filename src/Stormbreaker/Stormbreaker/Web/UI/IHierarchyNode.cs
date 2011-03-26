using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Web.UI {
    public interface IHierarchyNode<T> where T : IPageModel {
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        T Entity { get; set; }
        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        IEnumerable<IHierarchyNode<T>> ChildNodes { get; set; }
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        int Depth { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IHierarchyNode&lt;T&gt;"/> is expanded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if expanded; otherwise, <c>false</c>.
        /// </value>
        bool Expanded { get; set; }
    }
}