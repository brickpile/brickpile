using Stormbreaker.Common;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Dashboard.Web {
    public class DashboardPathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IPageRepository _repository;
        private IPageModel _pageModel;

        public IPathData ResolvePath(string virtualUrl) {
            
            // Set the default action to index
            _pathData.Action = PageRoute.DefaultAction;
            // Get an up to date page repository
            _repository = ObjectFactory.GetInstance<IPageRepository>();
            
            // Load and return the start page if the path is empty
            if(string.IsNullOrEmpty(virtualUrl)) {
                _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
                if(_pageModel == null) {
                    return null;
                }
                _pathData.CurrentPageModel = _pageModel;
                _pathData.Controller = _pageModel.GetControllerName();
                return _pathData;
            }

            // The normal beahaviour should be to load the page based on the url
            _pageModel = _repository.GetPageByUrl<IPageModel>(virtualUrl);
            // Try to load the page without the last segment of the url and set the last segment as action))
            if (_pageModel == null) {
                var index = virtualUrl.LastIndexOf("/");
                // The url does not contain any action or the action is the url
                if (index < 1) {
                    _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
                    _pathData.Action = virtualUrl.Trim(new[] {'/'});
                }
                else {
                    var path = virtualUrl.Substring(0, index).TrimStart(new[] { '/' });
                    _pageModel = _repository.GetPageByUrl<IPageModel>(path);
                    _pathData.Action = virtualUrl.Substring(index, virtualUrl.Length - index).Trim(new[] {'/'});
                }
            }
            if(_pageModel == null) {
                return null;
            }

            _pathData.CurrentPageModel = _pageModel;
            _pathData.Controller = _pathData.CurrentPageModel.GetControllerName();
            return _pathData;
        }

        public DashboardPathResolver(IPathData pathData, IPageRepository repository) {
            _pathData = pathData;
            _repository = repository;
        }
    }
}