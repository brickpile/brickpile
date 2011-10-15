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
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.Services;
using BrickPile.UI.Common;
using BrickPile.UI.Models;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using StructureMap;

namespace BrickPile.UI.App_Start {
    /// <summary>
    /// Bootstrapper for StructureMap
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class Bootstrapper {
        /// <summary>
        /// Configures StructureMap to look for registries.
        /// </summary>
        /// <returns></returns>
        public static IContainer Initialize() {
            
            ObjectFactory.Initialize(x => {

                var documentStore = new EmbeddableDocumentStore { ConnectionStringName = "RavenDB" };

                documentStore.Initialize();
                documentStore.Conventions.FindTypeTagName = type => typeof(IPageModel).IsAssignableFrom(type) ? "pages" : null;

                IndexCreation.CreateIndexes(typeof(Documents_ByParent).Assembly, documentStore);

                x.For<IDocumentStore>().Use(documentStore);

                x.For<IDocumentSession>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use(y =>
                    {
                        var store = y.GetInstance<IDocumentStore>();
                        return store.OpenSession();
                    });

                x.For<IVirtualPathResolver>().Use<VirtualPathResolver>();
                x.For<IPathResolver>().Use<PathResolver>();
                x.For<IPathData>().Use<PathData>();
                x.For<IPageRepository>().Use<PageRepository>();
                x.For<IRepository<IPageModel>>().Use<PageRepository>();

                x.For<IControllerMapper>().Use<ControllerMapper>();

                x.For<ISettings>().Use<Settings>();
                x.For<IPageModel>().UseSpecial(y => y.ConstructedBy( r => ((MvcHandler) HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentModel<IPageModel>()));
            });
            return ObjectFactory.Container;
        }
    }
}