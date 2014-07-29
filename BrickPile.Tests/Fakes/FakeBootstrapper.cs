using BrickPile.Core;
using BrickPile.Core.Conventions;
using Raven.Client;
using StructureMap;

namespace BrickPile.Tests.Fakes
{
    public class FakeBootstrapper : DefaultBrickPileBootstrapper
    {
        private readonly IDocumentStore _documentStore;

        public FakeBootstrapper(IContainer container, BrickPileConventions conventions) : base(container, conventions)
        {
        }

        public FakeBootstrapper(IContainer container, BrickPileConventions conventions, IDocumentStore documentStore)
            : base(container, conventions)
        {
            _documentStore = documentStore;
        }

        public override IDocumentStore InitialiseDocumentStore()
        {
            return _documentStore;
        }
    }
}
