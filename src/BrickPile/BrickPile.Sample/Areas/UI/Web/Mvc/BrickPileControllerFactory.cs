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
using System.Web.Routing;
using BrickPile.UI.Configuration;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.Web.Mvc {
    public class BrickPileControllerFactory : DefaultControllerFactory {
        private readonly IDocumentSession _session;
        private static bool _hasConfiguration;
        /// <summary>
        /// Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>
        /// The controller.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestContext"/> parameter is null.</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">The <paramref name="controllerName"/> parameter is null or empty.</exception>
        public override IController CreateController(RequestContext requestContext, string controllerName) {
            if (!HasConfiguration()) {
                requestContext.RouteData.DataTokens["area"] = "ui";
                requestContext.RouteData.Values["action"] = "index";
                requestContext.RouteData.Values["controller"] = "setup";
                return base.CreateController(requestContext, "setup");
            }

            return base.CreateController(requestContext, controllerName);
        }
        /// <summary>
        /// Determines whether this instance has configuration.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has configuration; otherwise, <c>false</c>.
        /// </returns>
        private bool HasConfiguration() {
            if(!_hasConfiguration) {
                var configuration = _session.Load<IConfiguration>("brickpile/configuration");
                _hasConfiguration = configuration != null;
            }
            return _hasConfiguration;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BrickPileControllerFactory"/> class.
        /// </summary>
        public BrickPileControllerFactory() {
            _session = ObjectFactory.GetInstance<IDocumentSession>();
        }
    }
}