using System.Configuration;
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
            var store = new EmbeddableDocumentStore
            {
                Conventions =
                {
                    FindTypeTagName = type => typeof (IPageModel).IsAssignableFrom(type) ? "Pages" : null
                },
                DataDirectory = "~/App_Data/Raven"
            };
            if (ConfigurationManager.ConnectionStrings["RavenDB"] != null)
            {
                store.ConnectionStringName = "RavenDB";
            }
            //store.RegisterListener(new StoreListener());
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(PageByUrl).Assembly, store);
            return store;
        }
    }
}