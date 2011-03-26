using Stormbreaker.Configuration;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    public class ConfigurationRegistry : Registry {
        public ConfigurationRegistry() {
            //For<IConfiguration>().Use<Configuration.Configuration>();    
        }
    }
}