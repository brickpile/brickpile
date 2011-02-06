using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Web.UI {
    public class HierarchyNode<T> : IHierarchyNode<T> where T : IPageModel
    {
        public T Entity { get; set; }
        public IEnumerable<IHierarchyNode<T>> ChildNodes { get; set; }
        public int Depth { get; set; }
        public bool Expanded { get; set; }
    }
}