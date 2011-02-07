using System.Linq;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Web {
    public class PathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IPageRepository _repository;
        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        public PathResolver(IPathData pathData, IRepository repository)
        {
            _pathData = pathData;
            _repository = (IPageRepository) repository;
        }
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        public IPathData ResolvePath(string virtualUrl)
        {

            if(string.IsNullOrEmpty(virtualUrl)) {
                return null;
            }

            _repository = (IPageRepository) ObjectFactory.GetInstance<IRepository>();

            _pathData.Action = PageRoute.DefaultAction;
            _pathData.CurrentPageModel = null;

            var slugs = virtualUrl.Split(new[] { '/' });
            
            IPageModel pageModel = null;
            for (var i = 0; i < slugs.Length; i++) {

                if (pageModel == null)
                {
                    pageModel = _repository.GetPageBySlug<IPageModel>(slugs[i]);
                }
                else
                {
                    var reference = pageModel.Children.Where(x => x.Slug == slugs[i]).FirstOrDefault();
                    pageModel = reference != null ? _repository.Load<IPageModel>(reference.Id) : null;
                }

                if (pageModel != null)
                {
                    _pathData.CurrentPageModel = pageModel;
                    continue;
                }
                _pathData.Action = slugs[i];
            }

            if (_pathData.CurrentPageModel == null)
            {
                return null;
            }

            _pathData.Controller = _pathData.CurrentPageModel.GetControllerName();
            return _pathData;
        }
    }
}