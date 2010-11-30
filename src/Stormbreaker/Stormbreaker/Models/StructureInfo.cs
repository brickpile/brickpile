using System.Collections.Generic;

namespace Stormbreaker.Models {
    public class StructureInfo : IStructureInfo {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public string Parent
        /// <summary>
        /// Get/Sets the Parent of the StructureInfo
        /// </summary>
        /// <value></value>
        public string ParentId { get; set; }
        #endregion
        #region public IList<string> Children
        /// <summary>
        /// Get/Sets the Children of the StructureInfo
        /// </summary>
        /// <value></value>
        public IList<string> Children { get; set; }
        #endregion
    }
}