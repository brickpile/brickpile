using System.Web.Mvc;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc;

namespace Stormbreaker.Controllers {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ContentController<T> : Controller where T : ContentItem {
        private readonly IRepository _repository;
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public T CurrentItem
        /// <summary>
        /// Gets the CurrentItem of the ContentController
        /// </summary>
        /// <value></value>
        public T CurrentItem
        {
            get
            {
                // Find a better solution to get the start page when th e
                if (_currentItem == null)
                {
                    _currentItem = RouteData.DataTokens[ContentRoute.ContentItemKey] as T;
                    if (_currentItem == null)
                    {
                        _currentItem = _repository.Get<T>(page => page.MetaData.UrlSegment == "hem");
                        RouteData.ApplyCurrentItem("Home", "Index", _currentItem);
                    }

                }
                return _currentItem;
            }
        }
        private T _currentItem;
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public ContentController(IRepository repository)
        /// <summary>
        /// Initializes a new instance of the <b>ContentController&lt;T&gt;</b> class.
        /// </summary>
        /// <param name="repository"></param>
        public ContentController(IRepository repository)
        {
            _repository = repository;
        }
        #endregion
    }
}