using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Web.UI {
    /// <summary>
    /// Represents the <see cref="StructureInfo" /> class
    /// </summary>
    public interface IStructureInfo {
        /// <summary>
        /// <see cref="StructureInfo.RootModel"/>
        /// </summary>
        IPageModel RootModel { get; }
        /// <summary>
        /// <see cref="StructureInfo.CurrentModel"/>
        /// </summary>
        IPageModel CurrentModel { get; }
        /// <summary>
        /// <see cref="StructureInfo.HierarchicalStructure"/>
        /// </summary>
        IEnumerable<IHierarchyNode<IPageModel>> HierarchicalStructure { get; }
    }
}