using System;

namespace Stormbreaker {
    /// <summary>
    /// Used to describe which Controller the ContentItem is using
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ControlsAttribute : Attribute {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public Type ItemType
        /// <summary>
        /// Get/Sets the ItemType of the ControlsAttribute
        /// </summary>
        /// <value></value>
        public Type ItemType { get; set; }
        #endregion
        #region public string ControllerName
        /// <summary>
        /// Gets the ControllerName of the ControlsAttribute
        /// </summary>
        /// <value></value>
        public string ControllerName
        {
            get
            {
                var name = ItemType.Name;
                var i = name.IndexOf("Controller");
                return i > 0 ? name.Substring(0, i) : name;
            }
        }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public ControlsAttribute(Type itemType)
        /// <summary>
        /// Initializes a new instance of the <b>ControlsAttribute</b> class.
        /// </summary>
        /// <param name="itemType"></param>
        public ControlsAttribute(Type itemType)
        {
            ItemType = itemType;
        }
        #endregion
    }
}
