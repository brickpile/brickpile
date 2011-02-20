using System.Collections.Generic;
using System.Linq;
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
            get { return _repository.Load<IPageModel>("pages/1"); }
        }
        /// <summary>
        ///   <see cref="StructureInfo.CurrentModel"/>
        /// </summary>
        public virtual IPageModel CurrentModel {
            get {
                return ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentModel<IPageModel>() ?? RootModel;
            }
        }
        /// <summary>
        ///   <see cref="StructureInfo.HierarchicalStructure"/>
        /// </summary>
        public virtual IEnumerable<IHierarchyNode<IPageModel>> HierarchicalStructure {
            get {
                if (CurrentModel == null)
                    return null;
                if(_hierarchicalStructure == null) {
                    var items = new List<IPageModel>();
                    items.AddRange(CurrentModel.Children.Select(child => _repository.Load<IPageModel>(child.Id)));
                    var ancestors = GetAncestors(CurrentModel);
                    foreach (var ancestor in ancestors) {
                        items.AddRange(_repository.GetChildren<IPageModel>(ancestor));
                    }
                    _hierarchicalStructure = items.CreateHierarchy(RootModel, CurrentModel, 0);
                }
                return _hierarchicalStructure.OrderByDescending(x => x.Entity.Id);
            }
        }
        private IEnumerable<IHierarchyNode<IPageModel>> _hierarchicalStructure;
        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private IEnumerable<IPageModel> GetAncestors(IPageModel item) {
            var parent = item.Parent != null ? _repository.Load<IPageModel>(item.Parent.Id) : null;

            if (parent != null) {
                yield return parent;
                if (parent.Parent != null){
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
