using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Stormbreaker.Resources;

namespace Stormbreaker.Models {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    [DebuggerDisplay("Name: {MetaData.Name} | Id: {Id} | Children Count { StructureInfo.Children.Count }")]
    public abstract class ContentItem : IContentItem {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public string Id
        /// <summary>
        /// Get/Sets the Id of the ContentItem
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        #endregion
        #region public IMetaData MetaData
        /// <summary>
        /// Get/Sets the MetaData of the ContentItem
        /// </summary>
        /// <value></value>
        public virtual IMetaData MetaData { get; private set; }
        #endregion
        #region public IStructureInfo StructureInfo
        /// <summary>
        /// Get/Sets the StructureInfo of the ContentItem
        /// </summary>
        /// <value></value>
        public virtual IStructureInfo StructureInfo { get; private set; }
        #endregion
        #region public virtual string Url
        /// <summary>
        /// Gets the Url of the ContentItem
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        [Display(Name = "Url", ResourceType = typeof(Common))]
        public virtual string Url
        {
            get { return MetaData.UrlSegment; }
        }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region protected ContentItem()
        /// <summary>
        /// Initializes a new instance of the <b>ContentItem</b> class.
        /// </summary>
        protected ContentItem()
        {
            MetaData = new MetaData();
            StructureInfo = new StructureInfo();
        }
        #endregion
    }
}