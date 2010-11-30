using System.Web.Mvc;
using Stormbreaker.Configuration;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Controllers {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ContentController<T> : Controller where T : ContentItem {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
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
                if (_currentItem == null)
                {
                    _currentItem = RouteData.DataTokens[ContentRoute.ContentItemKey] as T;
                    if (_currentItem == null)
                    {
                        _currentItem = (T)_repository.Get<ContentItem>(_configuration.StartItemId);
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
        #region public ContentController(IRepository repository, IConfiguration configuration)
        /// <summary>
        /// Initializes a new instance of the <b>ContentController&lt;T&gt;</b> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="configuration"></param>
        public ContentController(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        #endregion
    }
}