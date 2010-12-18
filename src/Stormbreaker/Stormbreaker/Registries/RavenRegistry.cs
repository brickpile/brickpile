using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Stormbreaker.Indexes;
using Stormbreaker.Models;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {

    public class RavenRegistry : Registry {
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        public RavenRegistry()
        {
            var documentStore = new DocumentStore()
                                    {
                                        Url = "http://localhost:8080"
                                    };

            documentStore.Initialize();
            documentStore.Conventions.FindTypeTagName = type => typeof(IDocument).IsAssignableFrom(type) ? "Documents" : null;
            IndexCreation.CreateIndexes(typeof(Document_BySlug).Assembly, documentStore);
            For<IDocumentStore>().Use(documentStore);
            For<IDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(x =>
                         {
                             var store = x.GetInstance<IDocumentStore>();
                             return store.OpenSession();
                         });
        }
    }
}