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
using BrickPile.UI.Models;
using Raven.Client;

namespace BrickPile.UI.Controllers {
    /// <summary>
    /// Common settings for BrickPile
    /// </summary>
    public class SettingsController : Controller {
        private readonly IDocumentSession _session;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            ViewBag.Class = "dashboard";
            var model = _session.Load<Settings>("brickpile/settings");
            return View(model);
        }
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Save(Settings model) {
            if(!ModelState.IsValid) {
                return View("index", model);
            }

            var settings = _session.Load<Settings>(model.Id);

            if(settings == null) {
                settings = new Settings() { Id = model.Id };
            }

            UpdateModel(settings);
            _session.Store(settings);
            _session.SaveChanges();

            return RedirectToAction("index");            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SettingsController(IDocumentSession session) {
            _session = session;
        }
    }
}
