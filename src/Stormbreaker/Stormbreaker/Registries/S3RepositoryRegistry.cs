using Stormbreaker.Repositories;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    public class S3RepositoryRegistry : Registry {
        public S3RepositoryRegistry() {
            For<IS3Repository>().Use<S3Repository>();
        }
    }
}
