using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Core.Infrastructure.Listeners;
using BrickPile.Domain.Models;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace BrickPile.UI {
    /// <summary>
    /// 
    /// </summary>
    public class RavenConfig {
        /// <summary>
        /// Initializes the raven.
        /// </summary>
        /// <returns></returns>
        public static DocumentStore InitializeRaven() {

            var documentStore = new EmbeddableDocumentStore
            {
                ConnectionStringName = "RavenDB",
                Conventions =
                {
                    FindTypeTagName = type => typeof (IPageModel).IsAssignableFrom(type) ? "Pages" : null
                }
            };

            documentStore.RegisterListener(new StoreListener());
            documentStore.Initialize();

            IndexCreation.CreateIndexes(typeof(PageByUrl).Assembly, documentStore);

            return documentStore;
        }
    }
}