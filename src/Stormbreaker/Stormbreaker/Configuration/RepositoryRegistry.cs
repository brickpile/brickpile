using Stormbreaker.Repositories;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Configuration {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class RepositoryRegistry : Registry {
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public RepositoryRegistry()
        /// <summary>
        /// Initializes a new instance of the <b>RepositoryRegistry</b> class.
        /// </summary>
        public RepositoryRegistry()
        {
            For<IRepository>().Use<Repository>();
        }
        #endregion
    }
}