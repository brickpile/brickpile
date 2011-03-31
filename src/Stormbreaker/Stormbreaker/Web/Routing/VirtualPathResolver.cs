using System.Web.Routing;
using Stormbreaker.Exceptions;
using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    public class VirtualPathResolver : IVirtualPathResolver {
        private string _action;
        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        public virtual string ResolveVirtualPath(IPageModel pageModel, RouteValueDictionary routeValueDictionary) {

            if(pageModel == null) {
                throw new StormbreakerException("A link cannot be created to a non existing page");
            }

            var url = pageModel.Parent == null ? string.Empty : pageModel.MetaData.Url;

            if (routeValueDictionary.ContainsKey(PageRoute.ActionKey)) {
                _action = routeValueDictionary[PageRoute.ActionKey] as string;
                if (!string.IsNullOrEmpty(_action) && !_action.Equals(PageRoute.DefaultAction)) {
                    return string.Format("{0}/{1}", url, _action);
                }
            }

            return string.Format("{0}", url);
        }
    }
}