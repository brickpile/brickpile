using Stormbreaker.Web;
using Stormbreaker.Web.Routing;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    public class PathDataRegistry : Registry {
        /* *******************************************************************
	    *  Constructors
	    * *******************************************************************/
        #region public PathDataRegistry()
        /// <summary>
        /// Initializes a new instance of the <b>PathDataRegistry</b> class.
        /// </summary>
        public PathDataRegistry()
        {
            For<IPathData>().Use<PathData>();
        }
        #endregion
    }
}