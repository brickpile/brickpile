using Stormbreaker.Web;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    public class PathResolverRegistry : Registry {
        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        #region public PathResolverRegistry()
        /// <summary>
        /// Initializes a new instance of the <b>PathResolverRegistry</b> class.
        /// </summary>
        public PathResolverRegistry()
        {
            Scan(a =>
            {
                a.AssembliesFromApplicationBaseDirectory();
                a.AddAllTypesOf<IPathResolver>();
            });
        }
        #endregion
    }
}