using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using Stormbreaker.Web.UI;
using StructureMap;

namespace Stormbreaker {
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
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.LookForRegistries();
                });
                x.For<IVirtualPathResolver>().Use<VirtualPathResolver>();
                x.For<IStructureInfo>().Use<StructureInfo>();
                x.For<IPathResolver>().Use<PathResolver>();
                x.For<IPathData>().Use<PathData>();
                x.For<IPageRepository>().Use<PageRepository>();
                x.For<IRepository<IPageModel>>().Use<PageRepository>();
            });
            return ObjectFactory.Container;
        }
    }
}