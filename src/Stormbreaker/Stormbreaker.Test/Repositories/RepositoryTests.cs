using Moq;
using NUnit.Framework;
using Raven.Client;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Test.Repositories
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void Add_SavesDocumentToSession()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyRepository repository = new DummyRepository(documentSessionMock.Object);
            
            IDocument document = new DummyDocument();
            repository.Store(document);

            documentSessionMock.Verify(x => x.Store(document), Times.Once());

        }

        [Test]
        public void Remove_DeletesDocumentFromSession()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyRepository repository = new DummyRepository(documentSessionMock.Object);

            IDocument document = new DummyDocument();
            repository.Delete(document);

            documentSessionMock.Verify(x => x.Delete(document), Times.Once());
        }

        [Test]
        public void Load_LoadsDocumentFromSession()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyRepository repository = new DummyRepository(documentSessionMock.Object);
            DummyDocument innerDoc = new DummyDocument();

            documentSessionMock.Setup(x => x.Load<DummyDocument>("testId")).Returns(innerDoc);

            IDocument document = repository.Load<DummyDocument>("testId");
            Assert.AreEqual(innerDoc, document);
        }
    }

    class DummyDocument : Document { }

    class DummyRepository : Repository
    {
        public DummyRepository(IDocumentSession session) : base(session) { }

    }
}
