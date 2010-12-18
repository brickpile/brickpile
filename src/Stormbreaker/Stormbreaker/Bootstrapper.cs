using StructureMap;

namespace Stormbreaker
{
    /// <summary>
    /// Bootstrapper for Structure map
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class Bootstrapperer
    {
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        public static void BootStrap(IContainer container) {
            container.Configure(x => x.Scan(a =>
            {
                a.AssembliesFromApplicationBaseDirectory();
                a.LookForRegistries();
            }));
        }
    }
}