using StructureMap;

namespace Stormbreaker
{
    /// <summary>
    /// Bootstrapper for Structure map
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class Bootstrapper {
        /// <summary>
        /// Configures the structure map.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void ConfigureStructureMap(IContainer container) {
            container.Configure(x => x.Scan(a =>
            {
                a.AssembliesFromApplicationBaseDirectory();
                a.LookForRegistries();
            }));
        }
    }
}