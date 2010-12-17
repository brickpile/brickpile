using System.Collections.Generic;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {
    public interface IViewModel<out T> where T : IDocument {
        T CurrentDocument { get; }
        IDocument RootDocument { get; }
        IEnumerable<HierarchyNode<IDocument>> StructureInfo { get; }
    }
}