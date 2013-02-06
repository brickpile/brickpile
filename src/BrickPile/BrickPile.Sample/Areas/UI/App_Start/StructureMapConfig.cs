using System.Web;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.App_Start {
    /// <summary>
    /// 
    /// </summary>
    public class StructureMapConfig {
        /// <summary>
        /// Initializes the structure map.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <returns></returns>
        public static IContainer InitializeStructureMap(IDocumentStore documentStore) {

            ObjectFactory.Initialize(x =>
            {

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

                x.For<IControllerMapper>().Use<ControllerMapper>();

                x.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory();
                    scanner.Convention<ContentTypeRegistrationConvetion>();
                });

                x.For<IPageModel>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentPage<IPageModel>()));

                x.For<IContent>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentContent<IContent>()));

                x.For<IStructureInfo>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetStructureInfo()));

            });

            return ObjectFactory.Container;
            
        }
    }
}