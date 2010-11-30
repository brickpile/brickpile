using System.Configuration;
using Stormbreaker.Configuration;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ConfigurationRegistry : Registry {
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public ConfigurationRegistry()
        /// <summary>
        /// Initializes a new instance of the <b>ConfigurationRegistry</b> class.
        /// </summary>
        public ConfigurationRegistry()
        {
            For<IConfiguration>()
                .Use((Configuration.Configuration)ConfigurationManager.GetSection("stormbreaker"));
        }
        #endregion
    }
}
