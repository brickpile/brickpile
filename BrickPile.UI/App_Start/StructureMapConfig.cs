using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Graph;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using StructureMap;
using StructureMap.Graph;

namespace BrickPile.UI {
    /// <summary>
    /// 
    /// </summary>
    public class StructureMapConfig {
        /// <summary>
        /// Initializes the structure map.
        /// </summary>
        /// <returns></returns>
        public static void ConfigureStructureMap() {

            ObjectFactory.Configure(x =>
            {

                x.For<IVirtualPathResolver>().Use<VirtualPathResolver>();

                x.For<IPathResolver>().Use<PathResolver>();

                x.For<IPathData>().Use<PathData>();

                x.For<IControllerMapper>().Use<ControllerMapper>();

                x.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory();
                    scanner.ExcludeNamespace("System");
                    scanner.ExcludeNamespace("Microsoft");
                    scanner.ExcludeNamespace("WebActivatorEx");
                    scanner.ExcludeNamespace("Newtonsoft");
                    scanner.ExcludeNamespace("Raven");
                    scanner.WithDefaultConventions();
                    scanner.Convention<ContentTypeRegistrationConvention>();
                });

                x.For<IPage>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentPage<IPage>()));

                x.For<IStructureInfo>().UseSpecial(y => y.ConstructedBy(r => ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetStructureInfo()));

            });
            
        }
    }
}