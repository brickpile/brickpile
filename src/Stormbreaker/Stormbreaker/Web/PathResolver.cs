using System.Linq;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Web {
    public class PathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IRepository _repository;
        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        public PathResolver(IPathData pathData, IRepository repository)
        {
            _pathData = pathData;
            _repository = repository;
        }
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        public IPathData ResolvePath(string virtualUrl)
        {
            // Hidden dependency, not good
            _repository = ObjectFactory.GetInstance<IRepository>();

            _pathData.Action = DocumentRoute.DefaultAction;

            var slugs = virtualUrl.Split(new[] { '/' });

            IDocument document = null;
            for (var i = 0; i < slugs.Length; i++) {

                if (document == null)
                {
                    document = _repository.LoadEntityBySlug<IDocument>(slugs[i]);
                }
                else
                {
                    var reference = document.Children.Where(x => x.Slug == slugs[i]).FirstOrDefault();
                    document = reference != null ? _repository.LoadEntityBySlug<IDocument>(reference.Slug) : null;
                }

                if (document != null)
                {
                    _pathData.CurrentDocument = document;
                    continue;
                }
                _pathData.Action = slugs[i];
            }

            if (_pathData.CurrentDocument == null)
            {
                return null;
            }

            _pathData.Controller = _pathData.CurrentDocument.GetControllerName();
            return _pathData;
        }
    }
}