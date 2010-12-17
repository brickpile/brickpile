using System.Collections.Generic;
using System.Linq;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {
    public class BaseViewModel<T> : IViewModel<T> where T : IDocument {
        private readonly IRepository _repository;
        /* *******************************************************************
	    *  Properties 
	    * *******************************************************************/
        public T CurrentDocument { get; set; }
        public IDocument RootDocument {
            get { return _repository.Load<IDocument>("documents/1"); }
        }
        public IEnumerable<HierarchyNode<IDocument>> StructureInfo
        {
            get
            {
                var items = new List<IDocument>();
                if (CurrentDocument.Equals(RootDocument))
                {
                    items.AddRange(_repository.GetChildren<IDocument>(CurrentDocument));
                }
                else {
                    var ancestors = GetAncestors(CurrentDocument);
                    items.AddRange(_repository.GetChildren<IDocument>(CurrentDocument));
                    foreach (var ancestor in ancestors) {
                        items.AddRange(_repository.GetChildren<IDocument>(ancestor));
                    }
                }
                return CreateHierarchy(items, RootDocument, 0);
            }
        }

        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        private IEnumerable<IDocument> GetAncestors(IDocument item)
        {
            var parent = item.Parent != null ? _repository.Load<IDocument>(item.Parent.Id) : null;

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

        private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity>(IEnumerable<TEntity> allItems, TEntity parentItem, int depth) where TEntity : IDocument {
            IEnumerable<TEntity> childs = allItems.Where(x => x.Parent.Id.Equals(parentItem.Id));

            if (childs.Count() > 0)
            {
                depth++;

                foreach (var item in childs)
                    yield return new HierarchyNode<TEntity>
                                     {
                        Entity = item,
                        ChildNodes = CreateHierarchy(allItems, item, depth),
                        Depth = depth
                    };
            }
        }
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        public BaseViewModel(T currentDocument, IRepository repository)
        {
            _repository = repository;
            CurrentDocument = currentDocument;
        }
    }
}