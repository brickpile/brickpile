using System.Collections.Generic;
using System.Linq;
using Stormbreaker.Configuration;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Web.UI.Navigation {
    public class NavigationInfo : INavigationInfo
    {
        private readonly IContentItem _currentItem;
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public IContentItem StartItem
        /// <summary>
        /// Gets the StartItem of the NavigationInfo
        /// </summary>
        /// <value></value>
        public IContentItem StartItem
        {
            get
            {
                return _repository.Get<IContentItem>(_configuration.StartItemId);
            }
        }
        #endregion
        #region public IContentItem CurrentItem
        /// <summary>
        /// Gets the CurrentItem of the NavigationInfo
        /// </summary>
        /// <value></value>
        public IContentItem CurrentItem
        {
            get { return _currentItem; }
        }
        #endregion
        #region public IEnumerable<INavigationItem> NavigationItems
        /// <summary>
        /// Gets the NavigationItems of the NavigationInfo
        /// </summary>
        /// <value></value>
        public virtual IEnumerable<INavigationItem> NavigationItems
        {
            get
            {
                if (_navigationItems == null)
                {
                    _navigationItems = new List<INavigationItem>();
                    var ancestors = GetAncestors(_currentItem, _configuration.StartItemId).Reverse();
                    foreach (var contentItem in ancestors)
                    {
                        // Add the root page
                        //_navigationItems.Add(new NavigationItem(contentItem));
                        AppendChildrenRecursive(contentItem, ancestors);
                    }
                }
                return _navigationItems;
            }
        }
        private IList<INavigationItem> _navigationItems;
        #endregion
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region private void AppendChildrenRecursive(IContentItem contentItem, IEnumerable<IContentItem> ancestors)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentItem"></param>
        /// <param name="ancestors"></param>
        private void AppendChildrenRecursive(IContentItem contentItem, IEnumerable<IContentItem> ancestors)
        {
            var children = ancestors.Contains(contentItem) ? _repository.GetChildrenFor(contentItem) : null;

            if (children == null)
            {
                return;
            }

            foreach (var item in children)
            {
                _navigationItems.Add(new NavigationItem(item));
                AppendChildrenRecursive(item, ancestors);
            }
            return;
        }
        #endregion
        #region private IEnumerable<IContentItem> GetAncestors(IContentItem item, string rootPageId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rootPageId"></param>
        /// <returns></returns>
        private IEnumerable<IContentItem> GetAncestors(IContentItem item, string rootPageId)
        {
            if (item.Id.Equals(rootPageId))
            {
                yield return item;
                yield break;
            }

            var parent = item.StructureInfo.ParentId != null ? _repository.Get<IContentItem>(item.StructureInfo.ParentId) : null;

            if (parent != null)
            {
                yield return parent;
                // Stop at given root page
                if (parent.StructureInfo.ParentId != null && !parent.Id.Equals(rootPageId) && !parent.StructureInfo.ParentId.Equals(rootPageId))
                {
                    foreach (var ancestor in GetAncestors(parent, rootPageId))
                    {
                        yield return ancestor;
                    }
                }
            }
        }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public NavigationInfo(IContentItem currentItem, IRepository repository, IConfiguration configuration)
        /// <summary>
        /// Initializes a new instance of the <b>NavigationInfo</b> class.
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="repository"></param>
        /// <param name="configuration"></param>
        public NavigationInfo(IContentItem currentItem, IRepository repository, IConfiguration configuration)
        {
            _currentItem = currentItem;
            _repository = repository;
            _configuration = configuration;
        }
        #endregion
    }
}