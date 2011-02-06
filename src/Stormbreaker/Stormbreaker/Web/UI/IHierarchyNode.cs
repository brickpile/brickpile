using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Web.UI {
    public interface IHierarchyNode<T> where T : IPageModel {
        T Entity { get; set; }
        IEnumerable<IHierarchyNode<T>> ChildNodes { get; set; }
        int Depth { get; set; }
        bool Expanded { get; set; }
    }
}