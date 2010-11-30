using Raven.Client;
using Raven.Client.Document;
using Stormbreaker.Models;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {

    public class RavenRegistry : Registry {
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public RavenRegistry()
        /// <summary>
        /// Initializes a new instance of the <b>RavenRegistry</b> class.
        /// </summary>
        public RavenRegistry()
        {
            var documentStore = new DocumentStore
                                    {
                                        Url = "http://localhost:8080",
                                        Conventions =
                                            {
                                                FindTypeTagName = type => typeof(IContentItem).IsAssignableFrom(type) ? "ContentItems" : null
                                            }
                                    };
            documentStore.Initialize();

            For<IDocumentStore>().Use(documentStore);
            For<IDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(x =>
                         {
                             var store = x.GetInstance<IDocumentStore>();
                             return store.OpenSession();
                         });
        }
        #endregion
    }
}