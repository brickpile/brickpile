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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Infrastructure.Common;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using Raven.Client;

namespace BrickPile.UI {
    /// <summary>
    /// Represents the navigation structure of the site
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class StructureInfo : IStructureInfo {
        private readonly IDocumentSession _session;
        private IPageModel _currentModel;
        private IPageModel _rootModel;
        private IPageModel _parentModel;
        private IEnumerable<IHierarchyNode<IPageModel>> _hierarchicalStructure;
        /// <summary>
        ///   <see cref="StructureInfo.RootModel"/>
        /// </summary>
        public virtual IPageModel RootModel {
            get {
                if(_rootModel == null) {
                    _rootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null);
                }
                return _rootModel;
            }
        }
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
        /// <summary>
        /// Gets the parent model.
        /// </summary>
        public virtual IPageModel ParentModel {
            get {
                if(_parentModel == null && CurrentModel != null && CurrentModel.Parent != null) {
                    _parentModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Id == CurrentModel.Parent.Id);
                }
                return _parentModel;
            }
        }
        /// <summary>
        /// Gets or sets the hierarchy.
        /// </summary>
        /// <value>
        /// The hierarchy.
        /// </value>
        public virtual IEnumerable<IHierarchyNode<IPageModel>> Hierarchy {
            get {
                if (CurrentModel == null)
                    return null;
                if (_hierarchicalStructure == null) {
                    _hierarchicalStructure = _session.LoadFrom<IPageModel>(x => x.Id == CurrentModel.Id)
                        .OrderBy(model => model.Metadata.SortOrder)
                        .AsHierarchy();
                }
                return _hierarchicalStructure;
            }
            set { _hierarchicalStructure = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureInfo"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public StructureInfo(IDocumentSession session) {
            _session = session;
        }
    }        
}
