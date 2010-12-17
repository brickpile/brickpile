using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stormbreaker.Configuration;
using Stormbreaker.Entities;
using Stormbreaker.Repositories;

namespace Stormbreaker.Web.UI {
    public class NavigationInfo {
        private readonly IContentItem _currentItem;
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;

        public INavigationItem StartItem
        {
            get { return NavigationItems.FirstOrDefault(); }
        }
        public IContentItem CurrentItem {
            get { return _currentItem; }
        }

        public IEnumerable<INavigationItem> NavigationItems {
            get {
                if(_navigationsItems == null) {
                    _navigationsItems = new Collection<INavigationItem>();
                    var ancestors = GetAncestors(_currentItem, "contentitems/21505").Reverse().ToList();
                    ancestors.Insert(0, _repository.Load<IContentItem>("contentitems/21505"));
                    foreach (var contentItem in ancestors) {
                        var navigationItem = new NavigationItem(contentItem);
                        _navigationsItems.Add(navigationItem);
                        AppendChildren(navigationItem.Children, contentItem,x => x.StructureInfo.Children);
                    }
                }
                return _navigationsItems;
            } 
        }
        private Collection<INavigationItem> _navigationsItems;

        private void AppendChildren(ICollection<INavigationItem> collection, IContentItem root, Func<IContentItem, IList<DenormalizedReference<IContentItem>>> childrenProperty)
        {
            var children = childrenProperty(root);
            if (children.Count == 0) {
                return;
            }

            foreach (var item in children) {
                var contentItem = _repository.Load<IContentItem>(item.Id);
                var navigationItem = new NavigationItem(contentItem);
                collection.Add(navigationItem);
                AppendChildren(navigationItem.Children, contentItem, childrenProperty);
            }
        }

        private IEnumerable<IContentItem> GetAncestors(IContentItem item, string rootPageId)
        {
            var parent = item.StructureInfo.Parent != null ? _repository.Load<IContentItem>(item.StructureInfo.Parent.Id) : null;

            if (parent != null)
            {
                yield return parent;
                // Stop at given root page
                if (parent.StructureInfo.Parent != null && !parent.Id.Equals(rootPageId) && !parent.StructureInfo.Parent.Id.Equals(rootPageId))
                {
                    foreach (var ancestor in GetAncestors(parent, rootPageId))
                    {
                        yield return ancestor;
                    }
                }
            }
        }        

        public NavigationInfo(IContentItem currentItem, IRepository repository, IConfiguration configuration) {
            _currentItem = currentItem;
            _repository = repository;
            _configuration = configuration;
        }
    }
}