using System;
using System.Diagnostics;

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
        public string Id { get; private set; }
        #endregion
        #region public IMetaData MetaData
        /// <summary>
        /// Get/Sets the MetaData of the ContentItem
        /// </summary>
        /// <value></value>
        public IMetaData MetaData { get; set; }
        #endregion
        #region public IStructureInfo StructureInfo
        /// <summary>
        /// Get/Sets the StructureInfo of the ContentItem
        /// </summary>
        /// <value></value>
        public IStructureInfo StructureInfo { get; set; }
        #endregion
        #region public string Url
        /// <summary>
        /// Get/Sets the Url of the ContentItem
        /// </summary>
        /// <value></value>
        public string Url { get; set; }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region protected ContentItem(string name)
        /// <summary>
        /// Initializes a new instance of the <b>ContentItem</b> class.
        /// </summary>
        /// <param name="name"></param>
        protected ContentItem(string name)
        {
            // transform the name to look the same as the urlsegment
            Id = string.Format("contentitems/{0}", name);
            MetaData = new MetaData();
            StructureInfo = new StructureInfo();
        }
        #endregion
    }
}