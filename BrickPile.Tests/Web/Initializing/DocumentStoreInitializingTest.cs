using BrickPile.Core.Infrastructure.Listeners;
using NUnit.Framework;
using System.Linq;
using Raven.Client;
using Raven.Client.Embedded;
using StructureMap;
using WebActivatorEx;

namespace BrickPile.Tests.Web.Initializing {
    public class DocumentStoreInitializingTest {
        private IDocumentStore _store;
        [SetUp]
        public void Setup() {
            _store = new EmbeddableDocumentStore {
                RunInMemory = true
            };
        }
        [Test]
        public void Can_Replace_Default_DocumentStore()
        {
            ObjectFactory.Initialize(x => x.For<IDocumentStore>().Use(_store));
            ActivationManager.RunPreStartMethods();

            var documentStoreFromStructureMap = ObjectFactory.GetInstance<IDocumentStore>();
            //The RunPreStartMethods Initializer registeres this StoreListener. Make sure its registered in this EmbeddableDocumentStore
            Assert.True(((EmbeddableDocumentStore)_store).RegisteredStoreListeners.Any(x => x is StoreListener));

            Assert.AreSame(_store,documentStoreFromStructureMap);
        }

    }
}
