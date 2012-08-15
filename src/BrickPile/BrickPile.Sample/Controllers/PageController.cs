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

using System.Web;
using System.Web.Mvc;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Common;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;
using Raven.Client.Document;

namespace BrickPile.Sample.Controllers {
    /// <summary>
    /// 
    /// </summary>
    public class PageController : Controller {
        private readonly Page _currentPage;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            var viewModel = new DefaultViewModel<Page>
            {
                CurrentPage = _currentPage,
                NavigationContext = _structureInfo.NavigationContext
            };

            ViewBag.Class = "page";

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageController"/> class.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="structureInfo">The structure info.</param>
        public PageController(Page currentPage, IStructureInfo structureInfo) {
            _currentPage = currentPage;
            _structureInfo = structureInfo;
        }
    }
}
