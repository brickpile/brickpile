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

using System.Web.Mvc;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller {
        private readonly IDocumentSession _documentSession;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public ActionResult Index(Home currentPage) {

            var quotePage = currentPage.QuoteLink.Id != null ?
                _documentSession.Load<BaseModel>(currentPage.QuoteLink.Id) :
                null;
            var viewModel = new HomeViewModel
                                {
                                    CurrentPage = currentPage,
                                    Pages = _structureInfo.Pages,
                                    QuotePage = quotePage,
                                    RootPage = _structureInfo.StartPage
                                };

            ViewBag.Class = "home";

            return View(viewModel);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="documentSession">The documentSession.</param>
        /// <param name="structureInfo">The structure info.</param>
        public HomeController(IDocumentSession documentSession, IStructureInfo structureInfo) {
            _documentSession = documentSession;
            _structureInfo = structureInfo;

        }
    }
}
