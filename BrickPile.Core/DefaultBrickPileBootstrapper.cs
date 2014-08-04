using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Conventions;
using BrickPile.Core.Extensions;
using BrickPile.Core.Graph;
using BrickPile.Core.Infrastructure.Listeners;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Json.Linq;
using StructureMap;
using StructureMap.Graph;

namespace BrickPile.Core
{
    public class DefaultBrickPileBootstrapper : IBrickPileBootstrapper
    {
        private const string ConnectionStringName = "RavenDB";
        private const string DataDirectory = "~/App_Data/Raven";
        public const string StructureInfoDocumentId = "structureinfos/1";

        private readonly BrickPileConventions conventions;

        protected IDocumentStore DocumentStore { get; private set; }
        protected IContainer ApplicationContainer { get; set; }

        protected virtual BrickPileConventions Conventions
        {
            get { return this.conventions; }
        }

        protected DefaultBrickPileBootstrapper()
        {
            this.conventions = new BrickPileConventions();
        }

        /// <summary>
        ///     Initialises this instance.
        /// </summary>
        public void Initialise()
        {
            this.DocumentStore = this.InitialiseDocumentStore();

            this.ApplicationContainer = this.GetApplicationContainer();

            this.ConfigureApplicationContainerInternal(this.ApplicationContainer, this.DocumentStore);

            this.ConfigureApplicationContainer(this.ApplicationContainer);

            this.ConfigureConventions(this.Conventions);

            this.CreateDefaultDocuments(this.DocumentStore);

            this.RegisterCustomRoutes(RouteTable.Routes);

            // Register structuremap as dependency resolver
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(this.ApplicationContainer));

            // Set the dependency resolver for the web api
            GlobalConfiguration.Configuration.Services.Replace(typeof (IHttpControllerActivator),
                new StructureMapControllerActivator(this.ApplicationContainer));

            // Extended metadata provider handling GroupName on the DisplayAttribute
            ModelMetadataProviders.Current = new ExtendedDataAnnotationsModelMetadataProvider();

            ModelValidatorProviders.Providers.Add(new ContentTypeMetadataValidatorProvider());

            ModelMetadataProviders.Current = new MetadataProvider();

            ControllerBuilder.Current.SetControllerFactory(typeof (BrickPileControllerFactory));

            RouteTable.Routes.RouteExistingFiles = false;

            RouteTable.Routes.AppendTrailingSlash = true;

            RouteTable.Routes.LowercaseUrls = true;

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new RazorViewEngine());

            // Ensure secure by default
            GlobalFilters.Filters.Add(new AuthorizeContentAttribute(this.DocumentStore));

            // Add editor tools as global filter
            GlobalFilters.Filters.Add(new EditorControlsAttribute());
        }


        /// <summary>
        ///     Gets the Container instance - automatically set during initialise.
        /// </summary>
        protected IContainer GetApplicationContainer()
        {
            return ObjectFactory.Container;
        }

        /// <summary>
        ///     Setups the default documents.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        protected void CreateDefaultDocuments(IDocumentStore documentStore)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                var structureInfo = session.Load<StructureInfo>(StructureInfoDocumentId);

                if (structureInfo != null) return;
                structureInfo = new StructureInfo {Id = StructureInfoDocumentId};
                session.Store(structureInfo);
                session.SaveChanges();
            }
        }

        [Obsolete("not used atm", false)]
        internal void OnPageSave(string key, IPage currentPage, RavenJObject metadata)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                // load structure info
                var structureInfo = session.Load<StructureInfo>(StructureInfoDocumentId);

                if (structureInfo.RootNode == null)
                {
                    structureInfo.RootNode = new StructureInfo.Node
                    {
                        PageId = key
                    };
                }
                else
                {
                    StructureInfo.Node[] nodes = structureInfo.RootNode.Flatten(n => n.Children).ToArray();

                    StructureInfo.Node parent = currentPage.Parent != null
                        ? nodes.SingleOrDefault(
                            n =>
                                String.Equals(n.PageId, currentPage.Parent.Id, StringComparison.CurrentCultureIgnoreCase))
                        : null;

                    if (parent != null)
                    {
                        currentPage.Metadata.Slug = Slug.CreateSlug(currentPage);
                        currentPage.Metadata.Url = currentPage.Metadata.Slug.Insert(0,
                            VirtualPathUtility.AppendTrailingSlash(parent.Url ?? ""));

                        if (parent.Children.All(n => n.PageId != key.Replace("/draft", "")))
                        {
                            parent.Children.Add(new StructureInfo.Node
                            {
                                PageId = key.Replace("/draft", ""),
                                ParentId = parent.PageId,
                                Url = currentPage.Metadata.Url
                            });
                        }
                    }
                }

                session.SaveChanges();
            }
        }

        internal void OnPagePublish(string key, IPage currentPage, RavenJObject metadata)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                // load structure info
                var structureInfo = session.Load<StructureInfo>(StructureInfoDocumentId);

                if (structureInfo.RootNode == null)
                {
                    structureInfo.RootNode = new StructureInfo.Node
                    {
                        PageId = key.Replace("/draft", "")
                    };
                }
                else
                {
                    StructureInfo.Node[] nodes = structureInfo.RootNode.Flatten(n => n.Children).ToArray();

                    StructureInfo.Node parentNode = currentPage.Parent != null
                        ? nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(currentPage.Parent.Id))
                        : null;

                    StructureInfo.Node currentNode = nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(key));

                    if (currentNode != null)
                    {
                        if (parentNode != null)
                        {
                            currentPage.Metadata.Slug = Slug.CreateSlug(currentPage);

                            currentPage.Metadata.Url = currentPage.Metadata.Slug.Insert(0,
                                VirtualPathUtility.AppendTrailingSlash(parentNode.Url ?? ""));

                            // the currentPage has been moved so we are moving the node and rewrites the url for all child pages and the current node
                            if (parentNode.ParentId != currentPage.Parent.Id)
                            {
                                structureInfo.MoveTo(parentNode, currentNode);

                                IEnumerable<string> ids = currentNode.Flatten(x => x.Children).Select(x => x.PageId);
                                IPage[] pages = session.Load<IPage>(ids);
                                pages.ForEach(p => { p.Metadata.Url = structureInfo.Get(p.Id).Url; });
                            }

                            currentNode.Url = currentPage.Metadata.Url;
                        }
                    }
                    else if (parentNode != null)
                    {
                        if (parentNode.Children.All(n => n.PageId != key.Replace("/draft", "")))
                        {
                            currentPage.Metadata.Slug = Slug.CreateSlug(currentPage);

                            currentPage.Metadata.Url = currentPage.Metadata.Slug.Insert(0,
                                VirtualPathUtility.AppendTrailingSlash(parentNode.Url ?? ""));

                            parentNode.Children.Add(new StructureInfo.Node
                            {
                                PageId = key.Replace("/draft", ""),
                                ParentId = parentNode.PageId,
                                Url = currentPage.Metadata.Url
                            });
                        }
                    }
                }

                // Clean up any existing draft for this page
                if (session.Advanced.DocumentStore.Exists(key + "/draft"))
                {
                    var draft = session.Load<IPage>(key + "/draft");
                    session.Delete(draft);
                }

                session.SaveChanges();
            }
        }

        internal void OnPageUnPublish(string key, IPage currentPage, RavenJObject metadata) {}

        /// <summary>
        ///     Called when [document delete].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="page">The currentPage.</param>
        /// <param name="metadata">The metadata.</param>
        internal void OnDocumentDelete(string key, IPage page, RavenJObject metadata)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                var structureInfo = session.Load<StructureInfo>(StructureInfoDocumentId);

                StructureInfo.Node node = structureInfo.Get(key);

                if (node != null)
                {
                    structureInfo.Delete(node);
                }

                // Clean up any existing draft for this page
                if (session.Advanced.DocumentStore.Exists(key + "/draft"))
                {
                    var draft = session.Load<IPage>(key + "/draft");
                    session.Delete(draft);
                }

                session.SaveChanges();
            }
        }

        /// <summary>
        ///     Registers the custom route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        protected void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.Add("PageRoute",
                new PageRoute(
                    new VirtualPathResolver(),
                    new RouteResolver(),
                    () => this.DocumentStore,
                    new ControllerMapper()));
        }

        /// <summary>
        ///     Configures the application container with registrations needed for BrickPile to work properly
        /// </summary>
        /// <param name="existingContainer">The existing container.</param>
        /// <param name="documentStore">The document store.</param>
        protected void ConfigureApplicationContainerInternal(IContainer existingContainer, IDocumentStore documentStore)
        {
            existingContainer.Configure(expression =>
            {
                expression.For<IDocumentStore>()
                    .Singleton()
                    .Use(documentStore);
                expression.For<IRouteResolver>()
                    .Use<RouteResolver>();
                expression.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory();
                    scanner.ExcludeNamespace("System");
                    scanner.ExcludeNamespace("Microsoft");
                    scanner.ExcludeNamespace("WebActivatorEx");
                    scanner.ExcludeNamespace("Newtonsoft");
                    scanner.ExcludeNamespace("Raven");
                    scanner.WithDefaultConventions();
                    scanner.Convention<ContentTypeRegistrationConvention>();
                });
                expression.For<IPage>()
                    .UseSpecial(
                        x =>
                            x.ConstructedBy(
                                () =>
                                    ((MvcHandler) HttpContext.Current.Handler).RequestContext.RouteData
                                        .GetCurrentPage<IPage>()));
                expression.For<INavigationContext>()
                    .UseSpecial(
                        x =>
                            x.ConstructedBy(
                                () => new NavigationContext(((MvcHandler) HttpContext.Current.Handler).RequestContext)));
            });
        }

        /// <summary>
        ///     Configures the application container with any additional registrations
        /// </summary>
        /// <param name="existingContainer">The existing container.</param>
        public virtual void ConfigureApplicationContainer(IContainer existingContainer) {}

        /// <summary>
        ///     Overrides/configures BrickPile's conventions
        /// </summary>
        /// <param name="brickPileConventions">The brick pile conventions.</param>
        public virtual void ConfigureConventions(BrickPileConventions brickPileConventions)
        {
            //conventions.VirtualPathProviderConventions.Register("Default", () => new NativeVirtualPathProvider());
        }

        /// <summary>
        ///     Initialises the document store.
        /// </summary>
        /// <returns></returns>
        public virtual IDocumentStore InitialiseDocumentStore()
        {
            var store = new EmbeddableDocumentStore
            {
                DataDirectory = DataDirectory
            };
            if (ConfigurationManager.ConnectionStrings[ConnectionStringName] != null)
            {
                store.ConnectionStringName = ConnectionStringName;
            }
            store.RegisterListener(new StoreListener(this.OnPagePublish, this.OnPageSave, this.OnPageUnPublish));
            store.RegisterListener(new DeleteListener(this.OnDocumentDelete));
            store.Initialize();
            IndexCreation.CreateIndexes(typeof (DefaultBrickPileBootstrapper).Assembly, store);
            return store;
        }
    }
}