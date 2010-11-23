using System.Web.Routing;
using Stormbreaker.Models;

namespace Stormbreaker.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class RouteExtensions {
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region public static RouteData ApplyCurrentItem(this RouteData data, string controllerName, string actionName, IContentItem item)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static RouteData ApplyCurrentItem(this RouteData data, string controllerName, string actionName, IContentItem item)
        {
            data.Values[ContentRoute.ControllerKey] = controllerName;
            data.Values[ContentRoute.ActionKey] = actionName;
            data.DataTokens[ContentRoute.ContentItemKey] = item;
            return data;
        }
        #endregion
    }
}