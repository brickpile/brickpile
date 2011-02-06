using System.Collections.Generic;
using System.Linq;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {

    public class DefaultViewModel<T> : IViewModel<T> where T : IPageModel {

        private readonly IRepository _repository;

        public virtual T CurrentModel { get; private set; }

        public virtual IPageModel RootPageModel {
            get { return _repository.Load<IPageModel>("pages/1"); }
        }

        public virtual IEnumerable<HierarchyNode<IPageModel>> StructureInfo
        {
            get
            {
                var items = new List<IPageModel>();
                items.AddRange(CurrentModel.Children.Select(child => _repository.Load<IPageModel>(child.Id)));
                var ancestors = GetAncestors(CurrentModel);
                foreach (var ancestor in ancestors) {
                    items.AddRange(_repository.GetChildren<IPageModel>(ancestor));
                }
                return items.CreateHierarchy(RootPageModel, 0);
            }
        }

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

        public DefaultViewModel(T currentModel, IRepository repository)
        {
            _repository = repository;
            CurrentModel = currentModel;
        }
    }
}