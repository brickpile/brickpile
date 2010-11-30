using System.Configuration;

namespace Stormbreaker.Configuration {
    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class Configuration : ConfigurationSection, IConfiguration {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public string StartPageId
        /// <summary>
        /// Get/Sets the StartPageId of the Configuration
        /// </summary>
        /// <value></value>
        [ConfigurationProperty("startPageId", IsRequired = true)]
        public string StartItemId
        {
            get
            {
                return (string)this["startPageId"];
            }
            set
            {
                this["startPageId"] = value;
            }
        }
        #endregion
    }
}
