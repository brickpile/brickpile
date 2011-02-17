using System;
using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web;

namespace Dashboard.Web.Routing {
    public class DashboardVirtualPathResolver : IVirtualPathResolver {
        private string _action;
        /* *******************************************************************
	    *  Methods
	    * *******************************************************************/
        public string ResolveVirtualPath(IDocument document, RouteValueDictionary routeValueDictionary) {
            if (routeValueDictionary.ContainsKey(DashboardRoute.ActionKey))
            {
                _action = routeValueDictionary[DashboardRoute.ActionKey] as string;
                if (_action != null && !_action.Equals(DashboardRoute.DefaultAction))
                {
                    return string.Format("dashboard/{0}/{1}/", document.Url, _action);
                }
            }
            return string.Format("dashboard/{0}/", document.Url);
        }
    }
}