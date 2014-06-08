using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Infrastructure.Listeners;
using Raven.Client.Indexes;
using StructureMap;
using StructureMap.Web;

namespace BrickPile.Core
{
    public class DefaultBrickPileBootstrapper : IBrickPileBootstrapper
    {
        private const string ConnectionStringName = "RavenDB";
        private const string DataDirectory = "~/App_Data/Raven";

        /// <summary>
        /// Gets the document store.
        /// </summary>
        /// <value>
        /// The document store.
        /// </value>
        protected Raven.Client.IDocumentStore DocumentStore { get; private set; }

        /// <summary>
        ///     Gets the Container instance - automatically set during initialise.
        /// </summary>
        protected IContainer ApplicationContainer { get; private set; }

        public void Initialise() {

            this.DocumentStore = InitialiseDocumentStore();
            this.ApplicationContainer = InitializeApplicationContainerInternal(DocumentStore);

            ConfigureApplicationContainer(this.ApplicationContainer);

            RouteTable.Routes.RouteExistingFiles = false;
            RouteTable.Routes.AppendTrailingSlash = true;
            RouteTable.Routes.LowercaseUrls = true;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        /// <summary>
        /// Configures the application container with registrations needed for BrickPile to work properly
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        protected IContainer InitializeApplicationContainerInternal(Raven.Client.IDocumentStore documentStore)
        {
            ObjectFactory.Initialize(expression => {
                expression.For<Raven.Client.IDocumentStore>()
                    .Singleton()
                    .Use(documentStore);
                expression.For<Raven.Client.IDocumentSession>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use(container => container.GetInstance<Raven.Client.IDocumentStore>().OpenSession());
            });

            return ObjectFactory.Container;
        }

        /// <summary>
        ///     Initialises the document store.
        /// </summary>
        /// <returns></returns>
        public virtual Raven.Client.IDocumentStore InitialiseDocumentStore()
        {
            var store = new Raven.Client.Embedded.EmbeddableDocumentStore
            {
                DataDirectory = DataDirectory
            };
            if (ConfigurationManager.ConnectionStrings[ConnectionStringName] != null)
            {
                store.ConnectionStringName = ConnectionStringName;
            }
            store.RegisterListener(new StoreListener());
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(DefaultBrickPileBootstrapper).Assembly, store);
            return store;
        }

        /// <summary>
        ///     Configures the application container with any additional registrations
        /// </summary>
        /// <param name="existingContainer">The existing container.</param>
        public virtual void ConfigureApplicationContainer(IContainer existingContainer)
        {
        }
    }
}
