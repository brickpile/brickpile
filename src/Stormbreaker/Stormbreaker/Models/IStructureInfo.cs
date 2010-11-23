using System.Collections.Generic;

namespace Stormbreaker.Models {
    public interface IStructureInfo {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        IContentItem Parent { get; set; }
        IList<IContentItem> Children { get; set; }
    }
}