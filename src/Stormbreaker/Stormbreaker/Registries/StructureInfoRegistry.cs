using System.Web;
using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of <see cref="StructureInfoRegistry"/> class.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class StructureInfoRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureInfoRegistry"/> class.
        /// </summary>
        public StructureInfoRegistry() {
            For<IStructureInfo>().Use<StructureInfo>();
        }
    }
}