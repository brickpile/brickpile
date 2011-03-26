using RestSharp;
using Stormbreaker.Dropbox.Helpers;
using Stormbreaker.Repositories;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {

    public class DropboxRepositoryRegistry : Registry {
        public DropboxRepositoryRegistry() {
            For<IRestClient>().UseSpecial(x => x.ConstructedBy(y => new RestClient("https://api.dropbox.com")));
            For<IDropboxRepository>().Use<DropboxRepository>();
            For<IRequestHelper>().Use<RequestHelper>();
        }
    }
}