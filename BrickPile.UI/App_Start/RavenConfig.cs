using System.Configuration;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Core.Infrastructure.Listeners;
using BrickPile.Domain.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using StructureMap;

namespace BrickPile.UI {
    /// <summary>
    /// 
    /// </summary>
    public class RavenConfig {
        /// <summary>
        /// Initializes the raven.
        /// </summary>
        /// <returns></returns>
        public static DocumentStore InitializeRaven()
        {

            DocumentStore store;
            if (ObjectFactory.Model.HasDefaultImplementationFor<IDocumentStore>()) {
                store = (DocumentStore)ObjectFactory.GetInstance(typeof(IDocumentStore));
            }
            else {
                store = new EmbeddableDocumentStore {
                                    DataDirectory = "~/App_Data/Raven"
                                };
                if (ConfigurationManager.ConnectionStrings["RavenDB"] != null) {
                    store.ConnectionStringName = "RavenDB";
                }
            }
            store.RegisterListener(new StoreListener());
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(PageByUrl).Assembly, store);
            return store;
        }
    }
}