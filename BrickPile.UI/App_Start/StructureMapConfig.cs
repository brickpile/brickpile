using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Graph;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using Raven.Client;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;

namespace BrickPile.UI {
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
                    .Use(container => container.GetInstance<IDocumentStore>().OpenSession());


                x.For<IVirtualPathResolver>().Use<VirtualPathResolver>();

                x.For<IPathResolver>().Use<PathResolver>();

                x.For<IPathData>().Use<PathData>();

                x.For<IControllerMapper>().Use<ControllerMapper>();

                x.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory();
                    scanner.Convention<ContentTypeRegistrationConvetion>();
                });

                x.For<IPage>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentPage<IPage>()));

                x.For<IStructureInfo>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetStructureInfo()));

            });

            return ObjectFactory.Container;
            
        }
    }
}