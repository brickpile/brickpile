using Stormbreaker.Web;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of <see cref="PathResolver"/> class.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PathResolverRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathResolverRegistry"/> class.
        /// </summary>
        public PathResolverRegistry()
        {
            For<IPathResolver>().Use<PathResolver>();
        }
    }
}