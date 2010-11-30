using System.Linq;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Web {
    public class ContentItemPathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IRepository _repository;
        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        #region public ContentItemPathResolver(IPathData pathData, IRepository repository)
        /// <summary>
        /// Initializes a new instance of the <b>PathResolver</b> class.
        /// </summary>
        /// <param name="pathData"></param>
        /// <param name="repository"></param>
        public ContentItemPathResolver(IPathData pathData, IRepository repository)
        {
            _pathData = pathData;
            _repository = repository;
        }
        #endregion
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region public IPathData ResolvePath(string virtualUrl)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualUrl"></param>
        /// <returns></returns>
        public IPathData ResolvePath(string virtualUrl)
        {
            _repository = ObjectFactory.GetInstance<IRepository>();

            _pathData.Action = ContentRoute.DefaultAction;

            var urlSegments = virtualUrl.Split(new[] { '/' });

            IContentItem item = null;
            for (var i = 0; i < urlSegments.Length; i++)
            {

                if (item == null)
                {
                    item = _repository.Get<IContentItem>(page => page.MetaData.UrlSegment == urlSegments[i]);
                }
                else
                {
                    var children = _repository.GetChildrenFor(item);
                    item = children.SingleOrDefault(x => x.MetaData.UrlSegment == urlSegments[i]);
                }

                if (item != null)
                {
                    _pathData.CurrentItem = item;
                    continue;
                }
                _pathData.Action = urlSegments[i];
            }

            if (_pathData.CurrentItem == null || !_pathData.CurrentItem.GetType().GetAttribute<ControlsAttribute>().ItemType.HasAction(_pathData.Action))
            {
                return null;
            }
            _pathData.Controller = _pathData.CurrentItem.GetControllerName();
            return _pathData;
        }
        #endregion
    }
}