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
using BrickPile.Core.Hosting;
using BrickPile.Core.Infrastructure.Listeners;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing;
using BrickPile.Core.Routing.Trie;
using BrickPile.Domain.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using StructureMap;
using StructureMap.Graph;

namespace BrickPile.Core
{
    /// <summary>
    ///     Responsible for handling the default initialisation of BrickPile
    /// </summary>
    public class DefaultBrickPileBootstrapper : IBrickPileBootstrapper
    {
        private const string ConnectionStringName = "RavenDB";
        public const string TrieId = "brickpile/trie";

        private static readonly Lazy<IDocumentStore> DocStore = new Lazy<IDocumentStore>(() =>
        {
            var store = new DocumentStore
            {
                Urls = new []{ ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString },
                Database = "brickpile"
            };

            var listener = new StoreListener(OnPagePublish, OnPageSave, OnPageUnPublish);
            store.OnBeforeStore += (sender, args) =>
            {
                listener.BeforeStore(args.DocumentId, args.Entity, args.DocumentMetadata, null);
            };            
            store.Initialize();
            return store;
        });

        /// <summary>
        ///     Gets the document store.
        /// </summary>
        /// <value>
        ///     The document store.
        /// </value>
        public static IDocumentStore DocumentStore
        {
            get { return DocStore.Value; }
        }

        /// <summary>
        ///     Gets or sets the application container.
        /// </summary>
        /// <value>
        ///     The application container.
        /// </value>
        protected IContainer Container { get; set; }

        /// <summary>
        ///     Gets the conventions.
        /// </summary>
        /// <value>
        ///     The conventions.
        /// </value>
        protected BrickPileConventions Conventions { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBrickPileBootstrapper" /> class.
        /// </summary>
        public DefaultBrickPileBootstrapper()
        {
            this.Conventions = new BrickPileConventions();
        }

        /// <summary>
        ///     Initialises BrickPile.
        /// </summary>
        public void Initialise()
        {
            this.ConfigureDocumentStoreInternal(DocumentStore);

            this.ConfigureDocumentStore(DocumentStore);

            this.Container = this.GetApplicationContainer();

            this.ConfigureApplicationContainerInternal(this.Container, DocumentStore);

            this.ConfigureApplicationContainer(this.Container);

            this.ConfigureConventions(this.Conventions);

            this.CreateDefaultDocuments(DocumentStore);

            this.RegisterRoutes(RouteTable.Routes);

            // Register structuremap as dependency resolver
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(this.Container));

            // Set the dependency resolver for the web api
            GlobalConfiguration.Configuration.Services.Replace(typeof (IHttpControllerActivator),
                new StructureMapControllerActivator(this.Container));

            // Extended metadata provider handling GroupName on the DisplayAttribute
            ModelMetadataProviders.Current = new ExtendedDataAnnotationsModelMetadataProvider();

            ModelValidatorProviders.Providers.Add(new ContentTypeMetadataValidatorProvider());

            ModelMetadataProviders.Current = new MetadataProvider();

            ControllerBuilder.Current.SetControllerFactory(new BrickPileControllerFactory(
                DocumentStore,
                new DefaultBrickPileContextFactory(
                    new RouteResolverTrie(
                        () => new HttpContextWrapper(HttpContext.Current),
                        DocumentStore),
                    new DefaultNavigationContextFactory(
                        () => new HttpContextWrapper(HttpContext.Current).Request.RequestContext,
                        new RouteResolverTrie(
                            () => new HttpContextWrapper(HttpContext.Current),
                            DocumentStore),
                        DocumentStore)),
                new RouteResolverTrie(
                    () => new HttpContextWrapper(HttpContext.Current),
                    DocumentStore),
                new DefaultNavigationContextFactory(
                    () => new HttpContextWrapper(HttpContext.Current).Request.RequestContext,
                    new RouteResolverTrie(
                        () => new HttpContextWrapper(HttpContext.Current),
                        DocumentStore),
                    DocumentStore)));

            RouteTable.Routes.RouteExistingFiles = false;

            RouteTable.Routes.AppendTrailingSlash = true;

            RouteTable.Routes.LowercaseUrls = true;

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new RazorViewEngine());

            // Ensure secure by default
            GlobalFilters.Filters.Add(new AuthorizeContentAttribute(DocumentStore));
        }

        /// <summary>
        ///     Gets the Container instance - automatically set during initialise.
        /// </summary>
        /// <returns></returns>
        protected IContainer GetApplicationContainer()
        {
            return new Container();
        }

        /// <summary>
        ///     Setups the default documents.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        protected void CreateDefaultDocuments(IDocumentStore documentStore)
        {
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                var trie = session.Load<Trie>(TrieId);

                if (trie != null) return;
                trie = new Trie { Id = TrieId };                
                session.Store(trie);
                session.SaveChanges();
            }
        }

        /// <summary>
        ///     Called when [page publish].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="metadata">The metadata.</param>
        internal static void OnPagePublish(string key, IPage currentPage, IMetadataDictionary metadata)
        {
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                var trie = session.Load<Trie>(TrieId);

                if (trie.RootNode == null)
                {
                    trie.RootNode = new TrieNode
                    {
                        PageId = key.Replace("/draft", "")
                    };
                }
                else
                {
                    TrieNode[] nodes = trie.RootNode.Flatten(n => n.Children).ToArray();

                    TrieNode parentNode = currentPage.Parent != null
                        ? nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(currentPage.Parent.Id))
                        : null;

                    TrieNode currentNode = nodes.SingleOrDefault(n => n.PageId.CompareToIgnoreDraftId(key));

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
                                trie.MoveTo(parentNode, currentNode);

                                IEnumerable<string> ids = currentNode.Flatten(x => x.Children).Select(x => x.PageId);
                                IDictionary<string, IPage> pages = session.Load<IPage>(ids); // TODO handle null values
                                foreach (var page in pages.Values)
                                {
                                    page.Metadata.Url = trie.Get(page.Id).Url;
                                }                                
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

                            parentNode.Children.Add(new TrieNode
                            {
                                PageId = key.Replace("/draft", ""),
                                ParentId = parentNode.PageId,
                                Url = currentPage.Metadata.Url
                            });
                        }
                    }
                }

                // Clean up any existing draft for this page
                if (session.Advanced.Exists(key + "/draft"))
                {
                    var draft = session.Load<IPage>(key + "/draft");
                    session.Delete(draft);
                }

                session.SaveChanges();
            }
        }

        /// <summary>
        ///     Called when [page un publish].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="metadata">The metadata.</param>
        internal static void OnPageUnPublish(string key, IPage currentPage, IMetadataDictionary metadata) {}

        /// <summary>
        ///     Called when [document delete].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="page">The currentPage.</param>
        /// <param name="metadata">The metadata.</param>
        internal void OnDocumentDelete(string key, IPage page, IMetadataDictionary metadata)
        {
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                var trie = session.Load<Trie>(TrieId);

                TrieNode node = trie.Get(key);

                if (node != null)
                {
                    trie.Delete(node);
                }

                // Clean up any existing draft for this page
                if (session.Advanced.Exists(key + "/draft"))
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
        protected void RegisterRoutes(RouteCollection routes)
        {
            // ensure that the the PageRoute is first in the collection
            routes.Insert(0,
                new DefaultRoute(
                    new VirtualPathResolver(),
                    new DefaultRouteResolver(
                        new RouteResolverTrie(
                            () => new HttpContextWrapper(HttpContext.Current),
                            DocumentStore)),
                    DocumentStore,
                    new ControllerMapper()));

            routes.Insert(1,
                new UiRoute(
                    new VirtualPathResolver(),
                    new DefaultRouteResolver(
                        new RouteResolverTrie(
                            () => new HttpContextWrapper(HttpContext.Current),
                            DocumentStore)),
                    DocumentStore,
                    new ControllerMapper()));
        }

        /// <summary>
        ///     Configures the document store internal.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        private void ConfigureDocumentStoreInternal(IDocumentStore documentStore)
        {
            IndexCreation.CreateIndexes(typeof (DefaultBrickPileBootstrapper).Assembly, documentStore);
        }

        /// <summary>
        ///     Configures the application container with registrations needed for BrickPile to work properly
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="documentStore">The document store.</param>
        protected void ConfigureApplicationContainerInternal(IContainer container, IDocumentStore documentStore)
        {
            container.Configure(expression =>
            {
                expression.For<IDocumentStore>()
                    .Singleton()
                    .Use(documentStore);
                expression.For<IRouteResolver>()
                    .Use<DefaultRouteResolver>();
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
                                () =>
                                    new NavigationContext(((MvcHandler) HttpContext.Current.Handler).RequestContext)));
                expression.For<IBrickPileContext>()
                    .UseSpecial(
                        x => x.ConstructedBy(
                            () =>
                                new BrickPileContext(((MvcHandler) HttpContext.Current.Handler).RequestContext)
                            )
                    );
            });
        }

        /// <summary>
        ///     Configures the application container with any additional registrations
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void ConfigureApplicationContainer(IContainer container) {}

        /// <summary>
        ///     Overrides/configures BrickPile's conventions
        /// </summary>
        /// <param name="brickPileConventions">The brick pile conventions.</param>
        public virtual void ConfigureConventions(BrickPileConventions brickPileConventions)
        {
            if (!UnitTestDetector.IsRunningFromNunit)
            {
                brickPileConventions.VirtualPathProviderConventions.Register("static", () => new NativeVirtualPathProvider());    
            }            
        }

        /// <summary>
        ///     Configures the document store.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public virtual void ConfigureDocumentStore(IDocumentStore documentStore) {}

        #region may be removed

        [Obsolete("not used atm", false)]
        internal static void OnPageSave(string key, IPage currentPage, IMetadataDictionary metadata)
        {
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                var trie = session.Load<Trie>(TrieId);

                if (trie.RootNode == null)
                {
                    trie.RootNode = new TrieNode
                    {
                        PageId = key
                    };
                }
                else
                {
                    TrieNode[] nodes = trie.RootNode.Flatten(n => n.Children).ToArray();

                    TrieNode parent = currentPage.Parent != null
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
                            parent.Children.Add(new TrieNode
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

        #endregion
    }
}