using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.Mvc;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Web {
    /// <summary>
    /// Responsible for unlocking the virtual path including the action for the ContentItem
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ContentItemVirtualPathResolver : IVirtualPathResolver {
        private string _action;
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region public string ResolveVirtualPath(IContentItem contentItem, RouteValueDictionary routeValueDictionary)
        /// <summary>
        /// Resolves to url and action based on the routevaluecollection and current contentitem
        /// </summary>
        /// <param name="contentItem"></param>
        /// <param name="routeValueDictionary"></param>
        /// <returns>The path to to this item</returns>
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