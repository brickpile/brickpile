using System.Web.Routing;
using Moq;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Client;
using Raven.Client.Indexes;
using Stormbreaker.Example;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Test.Extensions;
using Stormbreaker.Test.Repositories;
using Stormbreaker.Web;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Test.Web.Routing {
    public class RouteTests {
        IDocumentStore store;
        [Test]
        public void Default_Route_Test()
        {
            //Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            //Act
            var context = MvcMockHelpers.FakeHttpContext("~/");
            var routeData = routes.GetRouteData(context);
            //Assert
            Assert.AreEqual("Home", routeData.Values["controller"], "Default controller is HomeController");
        }

        [SetUp]
        public void SetUp() {
            //store = new EmbeddableDocumentStore { RunInMemory = true };
            //IndexCreation.CreateIndexes(typeof(Stormbreaker.Indexes.Document_BySlug).Assembly, store);
        }

        [Test]
        public void Dashboard_Default_Route_Test()
        {
            Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
            DummyPageRepository pageRepository = new DummyPageRepository(documentSessionMock.Object);

            IPageModel pageModel = new DummyPageModel { MetaData = { Slug = "slug"}};

            pageRepository.Store(pageModel);
            
            //Arrange
            //var routes = new RouteCollection();
            //var repository = new Mock<IRepository>();
            var pathData = new Mock<PathData>();
            var pathResolver = new PathResolver(pathData.Object, pageRepository);
            //var virtualPathResolver = new Mock<IVirtualPathResolver>();
            //routes.RegisterDocumentRoute(pathResholver.Object, virtualPathResolver.Object);
            //Act
            var data = pathResolver.ResolvePath("~/slug");
            //var context = MvcMockHelpers.FakeHttpContext("~/Dashboard");
            //var routeData = routes.GetRouteData(context);
            //Assert
            //Assert.AreEqual("Dashboard", routeData.Values["controller"]);
            Assert.AreEqual("Dasboard",data.Controller);
        }        
    }
}