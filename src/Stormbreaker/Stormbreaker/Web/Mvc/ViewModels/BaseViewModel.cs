using Stormbreaker.Configuration;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI.Navigation;

namespace Stormbreaker.Web.Mvc.ViewModels {
    public class BaseViewModel<T> : IViewModel<T> where T : IContentItem {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public T CurrentItem
        /// <summary>
        /// Get/Sets the CurrentItem of the ContentItemViewModel
        /// </summary>
        /// <value></value>
        public T CurrentItem { get; set; }
        #endregion
        #region public virtual INavigationInfo NavigationInfo
        /// <summary>
        /// Gets the NavigationInfo of the BaseViewModel
        /// </summary>
        /// <value></value>
        public virtual INavigationInfo NavigationInfo
        {
            get
            {
                return new NavigationInfo(CurrentItem, _repository, _configuration);
            }
        }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public BaseViewModel(T currentItem, IRepository repository, IConfiguration configuration)
        /// <summary>
        /// Initializes a new instance of the <b>ContentItemViewModel&lt;TModel&gt;</b> class.
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="repository"></param>
        /// <param name="configuration"></param>
        public BaseViewModel(T currentItem, IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            CurrentItem = currentItem;
        }
        #endregion        
    }
}