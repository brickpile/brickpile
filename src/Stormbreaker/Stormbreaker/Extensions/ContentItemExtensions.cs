using Stormbreaker.Models;

namespace Stormbreaker.Extensions {
    public static class ContentItemExtensions {
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region public static string GetControllerName(this IContentItem item)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetControllerName(this IContentItem item)
        {
            return item.GetType().GetAttribute<ControlsAttribute>().ControllerName.ToLower();
        }
        #endregion
    }
}