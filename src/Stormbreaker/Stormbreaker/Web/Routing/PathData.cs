using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    public class PathData : IPathData {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public string Action
        /// <summary>
        /// Get/Sets the Action of the PathData
        /// </summary>
        /// <value></value>
        public string Action { get; set; }
        #endregion
        #region public string Controller
        /// <summary>
        /// Get/Sets the Controller of the PathData
        /// </summary>
        /// <value></value>
        public string Controller { get; set; }
        #endregion
        #region public ContentItem CurrentItem
        /// <summary>
        /// Get/Sets the CurrentItem of the PathData
        /// </summary>
        /// <value></value>
        public IContentItem CurrentItem { get; set; }
        #endregion
    }
}