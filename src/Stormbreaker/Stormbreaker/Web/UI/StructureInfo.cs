using System.Collections.Generic;
using System.Linq;
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
        private readonly IPageModel _model;

        /// <summary>
        /// Gets the RootPage of the Hierarchical structure
        /// </summary>
        /// <value></value>
        public virtual IPageModel RootModel
        {
            get { return _repository.Load<IPageModel>("pages/1"); }
        }
        /// <summary>
        /// Gets the CurrentModel of the StructureInfo
        /// </summary>
        /// <value></value>
        public virtual IPageModel CurrentModel
        {
            get { return _model; }
        }
        /// <summary>
        /// Gets the HierarchicalStructure of the StructureInfo
        /// </summary>
        /// <value></value>
        public virtual IEnumerable<IHierarchyNode<IPageModel>> HierarchicalStructure
        {
            get
            {
                if (_model == null)
                    return null;

                var items = new List<IPageModel>();
                items.AddRange(_model.Children.Select(child => _repository.Load<IPageModel>(child.Id)));
                var ancestors = GetAncestors(_model);
                foreach (var ancestor in ancestors)
                {
                    items.AddRange(_repository.GetChildren<IPageModel>(ancestor));
                }
                return items.CreateHierarchy(RootModel, _model, 0);
            }
        }
        /// <summary>
        /// Gets the ancestors of a page
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private IEnumerable<IPageModel> GetAncestors(IPageModel item)
        {
            var parent = item.Parent != null ? _repository.Load<IPageModel>(item.Parent.Id) : null;

            if (parent != null)
            {
                yield return parent;
                if (parent.Parent != null)
                {
                    foreach (var ancestor in GetAncestors(parent))
                    {
                        yield return ancestor;
                    }
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureInfo" /> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="model"></param>
        public StructureInfo(IRepository<IPageModel> repository, IPageModel model)
        {
            _repository = (IPageRepository) repository;
            _model = model;
        }
    }        
}
