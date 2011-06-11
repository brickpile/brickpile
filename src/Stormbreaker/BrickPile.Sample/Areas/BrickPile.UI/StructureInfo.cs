/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;

namespace BrickPile.UI {
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
                return _currentModel ??
                       (_currentModel =
                        ((MvcHandler) HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentModel<IPageModel>() ??
                        RootModel);
            }
        }
        private IPageModel _currentModel;
        /// <summary>
        ///   <see cref="StructureInfo.HierarchicalStructure"/>
        /// </summary>
        public virtual IEnumerable<IHierarchyNode<IPageModel>> HierarchicalStructure {
            get {
                if (CurrentModel == null)
                    return null;
                if (_hierarchicalStructure == null) {
                    var items = new List<IPageModel>();
                    items.AddRange(_repository.GetChildren(CurrentModel));
                    var ancestors = GetAncestors(CurrentModel);
                    foreach (var ancestor in ancestors) {
                        items.AddRange(_repository.GetChildren(ancestor));
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
