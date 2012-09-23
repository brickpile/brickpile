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

using System.Configuration;
using System.Web.Mvc;
using BrickPile.UI.Common;
using BrickPile.UI.Web.Routing;

namespace BrickPile.UI {
    public class UIAreaRegistration : AreaRegistration {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        /// <returns>The name of the area to register.</returns>
        public override string AreaName { get { return "UI"; } }
        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context) {
            // Map the back office route for handling pages
            context.Routes.MapUIRoute("Pages_Default",
                "ui/{controller}/{action}/{id}", 
                new
                {
                    controller = "UI",
                    action = "Index",
                    id = UrlParameter.Optional,
                    area = "UI"
                },
                new[] { typeof(Controllers.UIController).Namespace });

            context.MapRoute(
                "UI_Default",
                "ui/{controller}/{action}/{id}",
                new
                {
                    controller = "UI",
                    action = "Index", 
                    id = UrlParameter.Optional,
                    area = "UI"
                },
                new[] { typeof(Controllers.UIController).Namespace }
            );
        }
    }
}