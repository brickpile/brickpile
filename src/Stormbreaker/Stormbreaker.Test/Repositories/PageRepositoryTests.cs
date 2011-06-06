using Moq;
using NUnit.Framework;
using Raven.Client;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Test.Repositories
{
    [TestFixture]
    public class PageRepositoryTests
    {
        [Test]
        public void Add_SavesDocumentToSession()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyPageRepository pageRepository = new DummyPageRepository(documentSessionMock.Object);

            IPageModel pageModel = new DummyPageModel();
            pageRepository.Store(pageModel);

            documentSessionMock.Verify(x => x.Store(pageModel), Times.Once());

        }

        [Test]
        public void Remove_DeletesDocumentFromSession()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyPageRepository pageRepository = new DummyPageRepository(documentSessionMock.Object);

            IPageModel pageModel = new DummyPageModel();
            pageRepository.Delete(pageModel);

            documentSessionMock.Verify(x => x.Delete(pageModel), Times.Once());
        }

        [Test]
        public void Load_LoadsDocumentFromSession()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyPageRepository pageRepository = new DummyPageRepository(documentSessionMock.Object);
            DummyPageModel innerDoc = new DummyPageModel();

            documentSessionMock.Setup(x => x.Load<DummyPageModel>("testId")).Returns(innerDoc);

            IPageModel pageModel = pageRepository.SingleOrDefault<DummyPageModel>(x => x.Id == "testId");
            Assert.AreEqual(innerDoc, pageModel);
        }


    }
    
    class DummyPageModel : PageModel { }

    class DummyPageRepository : PageRepository
    {
        public DummyPageRepository(IDocumentSession session) : base(session) { }

    }
}
