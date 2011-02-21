using System.Text;
using System.Web.Routing;
using Stormbreaker.Configuration;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Web {
    public class VirtualPathResolver : IVirtualPathResolver {
        private readonly IConfiguration _configuration;
        private string _action;
        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        public string ResolveVirtualPath(IPageModel pageModel, RouteValueDictionary routeValueDictionary) {
            if(pageModel == null) {
                return null;
            }
            var url = new StringBuilder(250);
            url.Append(pageModel.MetaData.Slug);
            var repository = ObjectFactory.GetInstance<IPageRepository>();
            var parent = pageModel;
            while (parent.Id != _configuration.HomePageId && parent.Parent != null && parent.Parent.Id != _configuration.HomePageId)
            {
                parent = repository.Load<IPageModel>(parent.Parent.Id);
                url.Insert(0, string.Format("{0}/", parent.MetaData.Slug));
            }

            if (routeValueDictionary.ContainsKey(PageRoute.ActionKey))
            {
                _action = routeValueDictionary[PageRoute.ActionKey] as string;
                if (_action != null && !_action.Equals(PageRoute.DefaultAction))
                {
                    return string.Format("{0}/{1}/", url, _action);
                }
            }
            return string.Format("{0}/", url);
        }
        public VirtualPathResolver(IConfiguration configuration) {
            _configuration = configuration;
        }
    }
}