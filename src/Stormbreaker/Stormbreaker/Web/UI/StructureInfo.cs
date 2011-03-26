using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Web.UI {
    /// <summary>
    /// Represents the navigation structure of the site
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class StructureInfo : IStructureInfo {
        private readonly IPageRepository _repository;
        /// <summary>
        ///   <see cref="StructureInfo.RootModel"/>
        /// </summary>
        public virtual IPageModel RootModel {
            get {
                if(_rootModel == null) {
                    _rootModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
                }
                return _rootModel;
            }
        }
        private IPageModel _rootModel;
        /// <summary>
        ///   <see cref="StructureInfo.CurrentModel"/>
        /// </summary>
        public virtual IPageModel CurrentModel {
            get {
                if(_currentModel == null) {
                    _currentModel = ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentModel<IPageModel>() ?? RootModel;
                }
                return _currentModel;
            }
        }
        private IPageModel _currentModel;


        public virtual IEnumerable<IHierarchyNode<IPageModel>> HierarchicalStructure {
            get {
                if (CurrentModel == null)
                    return null;
                if (_hierarchicalStructure == null) {
                    var items = new List<IPageModel>();
                    items.AddRange(_repository.Children(CurrentModel));
                    var ancestors = GetAncestors(CurrentModel);
                    foreach (var ancestor in ancestors) {
                        items.AddRange(_repository.Children(ancestor));
                    }
                    _hierarchicalStructure = items.CreateHierarchy(RootModel, 0);
                }
                return _hierarchicalStructure;
            }
        }
        private IEnumerable<IHierarchyNode<IPageModel>> _hierarchicalStructure;
        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private IEnumerable<IPageModel> GetAncestors(IPageModel item) {

            var parent = item.Parent != null ? _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(item.Parent.Id)) : null;

            if (parent != null) {
                yield return parent;
                if (parent.Parent != null) {
                    foreach (var ancestor in GetAncestors(parent)) {
                        yield return ancestor;
                    }
                }
            }

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureInfo"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public StructureInfo(IPageRepository repository) {
            _repository = repository;
        }
    }        
}
