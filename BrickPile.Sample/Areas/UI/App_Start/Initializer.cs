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

using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.UI.App_Start;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using BrickPile.UI.Web.ViewModels;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Initializer), "Start")]
namespace BrickPile.UI.App_Start {
    public static class Initializer {
        /// <summary>
        /// Initializer for BrickPile
        /// </summary>
        public static void Start() {

            //Insure that Raven is setup
            var documentStore = RavenConfig.InitializeRaven();
            //Insure that Structuremap would inject dependecies for any ASP.NET controller created
            var container = StructureMapConfig.InitializeStructureMap(documentStore);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            RouteTable.Routes.IgnoreRoute("static/{*pathInfo}");
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            ControllerBuilder.Current.SetControllerFactory(typeof(BrickPileControllerFactory));

            // Register the default page route
            RouteTable.Routes.Add("Default_Pages", new PageRoute(
                                      "{*path}",
                                      new RouteValueDictionary(
                                          new
                                          {
                                              controller = "pages",
                                              action = "index"
                                          }),
                                      new MvcRouteHandler()));

            var binderProvider = new InheritanceAwareModelBinderProvider
            {
                {typeof (IContent), new ContentModelBinder()}
            };

            ModelBinderProviders.BinderProviders.Add(binderProvider);
        }
    }
}