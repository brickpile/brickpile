using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.Mvc;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class VirtualPathResolver : IVirtualPathResolver {
        private string _action;
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region public string ResolveVirtualPath(IContentItem contentItem, RouteValueDictionary routeValueDictionary)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentItem"></param>
        /// <param name="routeValueDictionary"></param>
        /// <returns></returns>
        public string ResolveVirtualPath(IContentItem contentItem, RouteValueDictionary routeValueDictionary)
        {
            if (routeValueDictionary.ContainsKey(ContentRoute.ActionKey))
            {
                _action = routeValueDictionary[ContentRoute.ActionKey] as string;
                if (_action != null && !_action.Equals(ContentRoute.DefaultAction))
                {
                    return string.Format("{0}/{1}/", contentItem.Url, _action);
                }
            }
            return string.Format("{0}/", contentItem.Url);
        }
        #endregion
    }
}