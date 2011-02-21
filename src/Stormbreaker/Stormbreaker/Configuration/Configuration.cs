using System.Configuration;

namespace Stormbreaker.Configuration {
    public class Configuration : IConfiguration {
        /// <summary>
        /// Gets the home page id.
        /// </summary>
        public string HomePageId {
            get { return ConfigurationManager.AppSettings["Stormbreaker/HomePageId"]; } 
        }
    }
}