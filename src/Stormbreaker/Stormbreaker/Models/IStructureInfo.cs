using System.Collections.Generic;

namespace Stormbreaker.Models {
    public interface IStructureInfo {
        string ParentId { get; set; }
        IList<string> Children { get; set; }
    }
}