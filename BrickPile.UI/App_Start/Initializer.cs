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

using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core;
using BrickPile.UI;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using StructureMap;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Initializer), "Start")]
namespace BrickPile.UI {
    public static class Initializer {

        private static IBrickPileBootstrapper _brickPileBootstrapper;
        private static bool _initialized;

        /// <summary>
        /// Initializer for BrickPile
        /// </summary>
        public static void Start() {

            // Initialize BrickPile using IBrickPileBootstrapper
            Initialize();
            
            // Configure StructureMap
            StructureMapConfig.ConfigureStructureMap();

            //Insure that Structuremap would inject dependecies for any ASP.NET controller created
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            RouteTable.Routes.IgnoreRoute("static/{*pathInfo}");

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

            ModelValidatorProviders.Providers.Add(new ContentTypeMetadataValidatorProvider());
            ModelMetadataProviders.Current = new MetadataProvider();
        }

        private static void Initialize()
        {
            if (_initialized)
            {
                throw new Exception("Unexpected second call to PreStart");
            }

            _initialized = true;

            // Get the first non-abstract implementation of IBrickPileBootstrapper if one exists in the
            // app domain. If none exist then just use the default one.
            var bootstrapperInterface = typeof(IBrickPileBootstrapper);
            var defaultBootstrapper = typeof(DefaultBrickPileBootstrapper);

            var locatedBootstrappers =
                from asm in AppDomain.CurrentDomain.GetAssemblies() // TODO ignore known assemblies like m$ and such
                from type in asm.GetTypes()
                where bootstrapperInterface.IsAssignableFrom(type)
                where !type.IsInterface
                where type != defaultBootstrapper
                select type;

            var bootStrapperType = locatedBootstrappers.FirstOrDefault() ?? defaultBootstrapper;

            _brickPileBootstrapper = (IBrickPileBootstrapper)Activator.CreateInstance(bootStrapperType);

            _brickPileBootstrapper.Initialise();
        }
    }
}