using System.Linq;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc;
using StructureMap;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IRepository _repository;
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public PathResolver(IPathData pathData, IRepository repository)
        /// <summary>
        /// Initializes a new instance of the <b>PathResolver</b> class.
        /// </summary>
        /// <param name="pathData"></param>
        /// <param name="repository"></param>
        public PathResolver(IPathData pathData, IRepository repository)
        {
            _pathData = pathData;
            _repository = repository;
        }
        #endregion
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

            string[] urlSegments = virtualUrl.Split(new[] { '/' });

            IContentItem item = null;
            for (var i = 0; i < urlSegments.Length; i++)
            {
                item = item != null ?
                    item.StructureInfo.Children.SingleOrDefault(page => page.MetaData.UrlSegment == urlSegments[i]) :
                    _repository.Get<IContentItem>(page => page.MetaData.UrlSegment == urlSegments[i]);
                if (item != null)
                {
                    _pathData.CurrentItem = item;
                    continue;
                }
                _pathData.Action = urlSegments[i];
            }

            //if (_pathData.CurrentModel == null || !_pathData.CurrentModel.GetType().GetAttribute<ControlsAttribute>().ItemType.HasAction(_pathData.Action))
            //{
            //    return null;
            //}
            _pathData.Controller = "Home";
            return _pathData;
        }
        #endregion
    }
}