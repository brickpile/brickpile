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
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.UI.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.UI.Controllers {
    public class DialogController : Controller {
        private readonly IDocumentSession _session;
        /// <summary>
        /// Edits the model reference.
        /// </summary>
        /// <returns></returns>
        public ActionResult EditModelReference() {
            var currentModel = _session.Query<IPageModel>()
                .Where(x => x.Parent == null)
                .SingleOrDefault();
            var viewModel = new EditModelReferenceModel
                                {
                                    RootModel = currentModel,
                                    CurrentModel = currentModel,
                                    BackAction = "Index",
                                    Message = "Foo",
                                    Children = _session.Query<IPageModel>()
                                        .Where(x => x.Parent.Id == currentModel.Id)
                                };
            return PartialView(viewModel);

        }
        /// <summary>
        /// Gets the children model.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult GetChildrenModel(string id) {
            var currentModel = _session.Load<IPageModel>(id);

            var rootModel = currentModel.Parent == null ? _session.Query<IPageModel>()
                .Where(x => x.Parent == null)
                .SingleOrDefault() : null;
            
            var viewModel = new EditModelReferenceModel
            {
                RootModel = rootModel,
                CurrentModel = currentModel,
                ParentModel = currentModel.Parent != null ? _session.Query<IPageModel>()
                    .Where(x => x.Id == currentModel.Parent.Id)
                    .SingleOrDefault() : null,
                BackAction = "Index",
                Message = "Foo",
                Children = _session.Query<IPageModel>()
                    .Where(x => x.Parent.Id == id)
            };

            return PartialView("_Menu",viewModel);  
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogController"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public DialogController(IDocumentSession session) {
            _session = session;
        }
    }
}