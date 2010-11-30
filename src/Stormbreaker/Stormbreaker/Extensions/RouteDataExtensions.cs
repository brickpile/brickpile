using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.Mvc;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Extensions {
    public static class RouteDataExtensions {
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region public static RouteData ApplyCurrentItem(this RouteData data, string controllerName, string actionName, IContentItem contentItem)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="contentItem"></param>
        /// <returns></returns>
        public static RouteData ApplyCurrentItem(this RouteData data, string controllerName, string actionName, IContentItem contentItem)
        {
            data.Values[ContentRoute.ControllerKey] = controllerName;
            data.Values[ContentRoute.ActionKey] = actionName;
            data.DataTokens[ContentRoute.ContentItemKey] = contentItem;
            return data;
        }
        #endregion
    }
}