using Raven.Client;
using Raven.Client.Client;
using Raven.Client.Indexes;
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
        public RavenRegistry()
        {
            var documentStore = new EmbeddableDocumentStore
                                    {
                                        DataDirectory = "~\\App_Data\\RavenDB",
                                        UseEmbeddedHttpServer = true
                                    };
            
            documentStore.Initialize();
            documentStore.Conventions.FindTypeTagName = type => typeof(IPageModel).IsAssignableFrom(type) ? "Pages" : null;

            IndexCreation.CreateIndexes(typeof(Document_BySlug).Assembly, documentStore);
            IndexCreation.CreateIndexes(typeof(Documents_ByParent).Assembly, documentStore);

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