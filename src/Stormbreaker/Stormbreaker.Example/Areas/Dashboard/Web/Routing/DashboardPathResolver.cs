using System.Linq;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web;
using StructureMap;

namespace Dashboard.Web.Routing {
    public class DashboardPathResolver : IPathResolver {

        private IRepository _repository;
        private readonly IPathData _pathData;
        public const string Controller = "Dashboard";

        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        #region public DashboardPathResolver(IRepository repository, IPathData pathData)
        /// <summary>
        /// Initializes a new instance of the <b>DashboardPathResolver</b> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pathData"></param>
        public DashboardPathResolver(IRepository repository, IPathData pathData)
        {
            _repository = repository;
            _pathData = pathData;
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
            // rebuild the instance of repository
            _repository = ObjectFactory.GetInstance<IRepository>();

            _pathData.Action = DashboardRoute.DefaultAction;

            var urlSegments = virtualUrl.Split(new[] { '/' });

            IContentItem item = null;
            for (var i = 1; i < urlSegments.Length; i++)
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

            if (_pathData.CurrentItem == null)
            {
                return null;
            }

            _pathData.Controller = Controller;
            return _pathData;
        }
        #endregion
    }
}