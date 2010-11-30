using System;
using System.ComponentModel.DataAnnotations;
using Stormbreaker.Resources;

namespace Stormbreaker.Models {
    public class MetaData : IMetaData {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public string Name
        /// <summary>
        /// Get/Sets the Name of the MetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "Name", ResourceType = typeof(Common), Order = 1)]
        [Required(ErrorMessageResourceName = "Name_Required", ErrorMessageResourceType = typeof(Common))]
        public string Name { get; set; }
        #endregion
        #region public DateTime? Created
        /// <summary>
        /// Get/Sets the Created of the MetaData
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public DateTime? Created { get; set; }
        #endregion
        #region public DateTime? Updated
        /// <summary>
        /// Get/Sets the Updated of the MetaData
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public DateTime? Updated { get; set; }
        #endregion
        #region public DateTime? StartPublish
        /// <summary>
        /// Get/Sets the StartPublish of the MetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "StartPublish", ResourceType = typeof(Common), Order = 2)]
        [DataType(DataType.DateTime)]
        [UIHint("_DateTime")]
        public DateTime? StartPublish { get; set; }
        #endregion
        #region public DateTime? StopPublish
        /// <summary>
        /// Get/Sets the StopPublish of the MetaData
        /// </summary>
        /// <value></value>
        [DataType(DataType.DateTime)]
        [UIHint("_DateTime")]
        [Display(Name = "StopPublish", ResourceType = typeof(Common), Order = 3)]
        public DateTime? StopPublish { get; set; }
        #endregion
        #region public string UrlSegment
        /// <summary>
        /// Get/Sets the UrlSegment of the MetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "UrlSegment", ResourceType = typeof(Common), Order = 5)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UrlSegment_Required", ErrorMessageResourceType = typeof(Common))]
        public string UrlSegment { get; set; }
        #endregion
    }
}