using System.Collections.Generic;

namespace Stormbreaker.Models {
    public class StructureInfo : IStructureInfo {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public IContentItem Parent
        /// <summary>
        /// Get/Sets the Parent of the StructureInfo
        /// </summary>
        /// <value></value>
        public IContentItem Parent { get; set; }
        #endregion
        #region public IList<IContentItem> Children
        /// <summary>
        /// Get/Sets the Children of the StructureInfo
        /// </summary>
        /// <value></value>
        public IList<IContentItem> Children { get; set; }
        #endregion
    }
}