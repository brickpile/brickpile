using Stormbreaker.Web.Routing;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Configuration {
    public class VirtualPathResolverRegistry : Registry {
        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        #region public VirtualPathResolverRegistry()
        /// <summary>
        /// Initializes a new instance of the <b>VirtualPathResolverRegistry</b> class.
        /// </summary>
        public VirtualPathResolverRegistry()
        {
            Scan(a =>
            {
                a.AssembliesFromApplicationBaseDirectory();
                a.AddAllTypesOf<IVirtualPathResolver>();
            });
        }
        #endregion
    }
}