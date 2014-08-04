using BrickPile.Core;
using Raven.Client;

namespace BrickPile.Tests.Fakes
{
    public class FakeBootstrapper : DefaultBrickPileBootstrapper
    {
        private readonly IDocumentStore documentStore;


        public FakeBootstrapper(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public override IDocumentStore InitialiseDocumentStore()
        {
            return this.documentStore;
        }
    }
}
