using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.Routing;

namespace Dashboard.Web {
    public class DashboardVirtualPathResolver : VirtualPathResolver {
        private string _action;
        public override string ResolveVirtualPath(IPageModel pageModel, RouteValueDictionary routeValueDictionary) {

            if (pageModel == null) {
                return null;
            }
            var url = pageModel.MetaData.Url ?? string.Empty;

            if (routeValueDictionary.ContainsKey(PageRoute.ActionKey)) {
                _action = routeValueDictionary[PageRoute.ActionKey] as string;
                if (_action != null && !_action.Equals(PageRoute.DefaultAction)) {
                    return string.Format("{0}/{1}", url, _action);
                }
            }
            return string.Format("{0}", url);
        }        
    }
}