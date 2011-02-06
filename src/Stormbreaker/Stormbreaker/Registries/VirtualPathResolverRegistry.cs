using Stormbreaker.Web;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of <see cref="VirtualPathResolverRegistry"/> class.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class VirtualPathResolverRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPathResolverRegistry"/> class.
        /// </summary>
        public VirtualPathResolverRegistry()
        {
            For<IVirtualPathResolver>().Use<VirtualPathResolver>();
        }
    }
}