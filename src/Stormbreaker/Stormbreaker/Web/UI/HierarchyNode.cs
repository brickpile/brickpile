using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Web.UI {
    public class HierarchyNode<T> where T : IDocument
    {
        public T Entity { get; set; }
        public IEnumerable<HierarchyNode<T>> ChildNodes { get; set; }
        public int Depth { get; set; }
    }
}