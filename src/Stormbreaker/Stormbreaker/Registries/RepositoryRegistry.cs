using Stormbreaker.Repositories;
using Stormbreaker.Web;
using Stormbreaker.Web.Routing;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    public class RepositoryRegistry : Registry {
        public RepositoryRegistry() {
            For<IRepository>().Use<Repository>();
            For<IPathResolver>().Use<PathResolver>();
            For<IVirtualPathResolver>().Use<VirtualPathResolver>();
            For<IPathData>().Use<PathData>();
        }
    }
}