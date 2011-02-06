using Stormbreaker.Web;
using Stormbreaker.Web.Routing;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of <see cref="PathDataRegistry"/> class.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PathDataRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDataRegistry"/> class.
        /// </summary>
        public PathDataRegistry()
        {
            For<IPathData>().Use<PathData>();
        }
    }
}