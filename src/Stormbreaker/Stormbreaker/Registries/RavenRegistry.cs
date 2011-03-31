using Raven.Client;
using Raven.Client.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Http;
using Stormbreaker.Indexes;
using Stormbreaker.Models;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of the <see cref="EmbeddableDocumentStore"/> class
    /// and wire up <see cref="IDocumentStore"/> and <see cref="IDocumentSession"/> to StructureMap
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class RavenRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="RavenRegistry"/> class.
        /// </summary>
        public RavenRegistry() {

            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8000);
            var documentStore = new DocumentStore() {Url = "http://localhost:8000"};
            //var documentStore = new EmbeddableDocumentStore
            //                        {
            //                            ConnectionStringName = "RavenConStr",
            //                            UseEmbeddedHttpServer = true
            //                        };
            
            documentStore.Initialize();
            documentStore.Conventions.FindTypeTagName = type => typeof(IPageModel).IsAssignableFrom(type) ? "Pages" : null;

            IndexCreation.CreateIndexes(typeof(Documents_ByParent).Assembly, documentStore);
            IndexCreation.CreateIndexes(typeof(Document_ByUrl).Assembly, documentStore);

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