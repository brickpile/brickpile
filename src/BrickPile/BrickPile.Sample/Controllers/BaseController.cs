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
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.UI;

namespace BrickPile.Sample.Controllers {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseController<T> : Controller where T : IPageModel {
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Base controller with Current model and a default hierarchy for navigational purpose
        /// </summary>
        private IEnumerable<IPageModel> _pages;
        /// <summary>
        /// Gets the current model.
        /// </summary>
        public T CurrentModel { get; private set; }
        /// <summary>
        /// Gets the pages.
        /// </summary>
        public virtual IEnumerable<IPageModel> Pages {
            get {
                return _pages
                       ?? (_pages = _structureInfo.Pages
                                            .Where(x => x.Metadata.IsPublished)
                                            .Where(x => !x.Metadata.IsDeleted)
                                            .Where(x => x.Metadata.DisplayInMenu)
                                            .OrderBy(x => x.Metadata.SortOrder));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        protected BaseController(IPageModel model, IStructureInfo structureInfo) {
            CurrentModel = (T)model;
            _structureInfo = structureInfo;
        }
    }
}
