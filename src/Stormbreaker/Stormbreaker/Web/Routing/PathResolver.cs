using System.Web;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using StructureMap;

namespace Stormbreaker.Web.Routing {
    public class PathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IPageRepository _repository;
        private IPageModel _pageModel;
        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="virtualUrl">The virtual URL.</param>
        /// <returns></returns>
        public IPathData ResolvePath(string virtualUrl) {
            // Set the default action to index
            _pathData.Action = PageRoute.DefaultAction;
            // Get an up to date page repository
            _repository = ObjectFactory.GetInstance<IPageRepository>();
            
            // Load and return the start page if the path is empty
            if(string.IsNullOrEmpty(virtualUrl)) {
                _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
                _pathData.CurrentPageModel = _pageModel;
                _pathData.Controller = _pageModel.GetControllerName();
                return _pathData;
            }

            VirtualPathUtility.RemoveTrailingSlash(virtualUrl);
            // The normal beahaviour should be to load the page based on the url
            _pageModel = _repository.ByUrl<IPageModel>(virtualUrl);

            // Try to load the page without the last segment of the url and set the last segment as action
            if (_pageModel == null) {
                var index = virtualUrl.LastIndexOf("/");
                var path = virtualUrl.Substring(0, index).TrimStart(new[] { '/' });
                _pageModel = _repository.ByUrl<IPageModel>(path);
                _pathData.Action = virtualUrl.Substring(index, virtualUrl.Length - index).Trim(new[] {'/'});
            }

            _pathData.CurrentPageModel = _pageModel;
            _pathData.Controller = _pathData.CurrentPageModel.GetControllerName();
            return _pathData;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathResolver"/> class.
        /// </summary>
        /// <param name="pathData">The path data.</param>
        /// <param name="repository">The repository.</param>
        public PathResolver(IPathData pathData, IPageRepository repository) {
            _pathData = pathData;
            _repository = repository;
        }
    }
}