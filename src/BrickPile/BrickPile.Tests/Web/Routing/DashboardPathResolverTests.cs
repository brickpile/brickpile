using System.Web.Mvc;
using BrickPile.Core.Repositories;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.UI.Controllers;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.Routing;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace BrickPile.Tests.Web.Routing {
    public class DashboardPathResolverTests {
        /// <summary>
        /// This test is based on a request for the start page
        /// </summary>
        /// <param name="path">The path.</param>
        [TestCase("")]
        [TestCase("/")]
        public void Home_Page_With_Default_Action(string path) {
            // Arrange
            var pathData = new PathData();
            var pageModel = new DummyModel();

            var repository = new Mock<IPageRepository>();
            var mapper = new Mock<IControllerMapper>();
            var container = new Mock<IContainer>();

            container.Setup(x => x.GetInstance<IPageRepository>()).Returns(repository.Object);
            repository.Setup(x => x.SingleOrDefault<IPageModel>(model => model.Parent == null)).Returns(pageModel);
            var resolver = new DashboardPathResolver(pathData,repository.Object,mapper.Object, container.Object);

            // Act
            var data = resolver.ResolvePath(path);

            // Assert
            Assert.NotNull(data);
            Assert.AreEqual("index", data.Action);
            Assert.AreEqual("Content",data.Controller);
        }
        /// <summary>
        /// This test is based on a request for the page with the url ~/page with the default action index
        /// </summary>
        /// <param name="virtualUrl">The virtual URL.</param>
        [TestCase("/page")]
        //[TestCase("/page/")]
        public void Page_With_Default_Action(string virtualUrl) {

            // Arrange
            var pathData = new PathData();
            var pageModel = new DummyModel();

            var repository = new Mock<IPageRepository>();
            var mapper = new Mock<IControllerMapper>();
            var container = new Mock<IContainer>();

            container.Setup(x => x.GetInstance<IPageRepository>()).Returns(repository.Object);
            repository.Setup(x => x.GetPageByUrl<IPageModel>(virtualUrl)).Returns(pageModel);
            mapper.Setup(m => m.GetControllerName(typeof(DummyController))).Returns("DummyModel");

            var resolver = new DashboardPathResolver(pathData, repository.Object, mapper.Object, container.Object);

            // Act
            var data = resolver.ResolvePath(virtualUrl);

            // Assert
            Assert.NotNull(data);
            Assert.AreEqual("index", data.Action);
            Assert.AreEqual("Content", data.Controller);            
        }

        /// <summary>
        /// This test is based on a request for the pate with url ~/page with the action myaction
        /// </summary>
        /// <param name="path">The path.</param>
        [TestCase("/page/myaction")]
        [TestCase("/page/myaction/")]
        public void Page_With_Custom_Action(string virtualUrl) {
            
            // Arrange
            var pathData = new PathData();
            var pageModel = new DummyModel();

            var repository = new Mock<IPageRepository>();
            var mapper = new Mock<IControllerMapper>();
            var container = new Mock<IContainer>();

            container.Setup(x => x.GetInstance<IPageRepository>()).Returns(repository.Object);
            repository.Setup(r => r.GetPageByUrl<IPageModel>("page")).Returns(pageModel);
            mapper.Setup(m => m.GetControllerName(typeof (DummyController))).Returns("DummyModel");

            var resolver = new DashboardPathResolver(pathData, repository.Object, mapper.Object, container.Object);

            // Act
            var data = resolver.ResolvePath(virtualUrl);

            // Assert
            Assert.NotNull(data);
            Assert.AreEqual("myaction", data.Action);
            Assert.AreEqual("Content", data.Controller);
        }

        [TestCase("/myaction")]
        [TestCase("/myaction/")]
        public void Home_Page_With_Custom_Action(string virtualUrl) {

            // Arrange
            var pathData = new PathData();
            var pageModel = new DummyModel();

            var repository = new Mock<IPageRepository>();
            var mapper = new Mock<IControllerMapper>();
            var container = new Mock<IContainer>();

            container.Setup(x => x.GetInstance<IPageRepository>()).Returns(repository.Object);
            repository.Setup(x => x.SingleOrDefault<IPageModel>(model => model.Parent == null)).Returns(pageModel);
            mapper.Setup(m => m.GetControllerName(typeof(ContentController))).Returns("Content");
            mapper.Setup(m => m.ControllerHasAction("Content", "myaction")).Returns(true);

            var resolver = new DashboardPathResolver(pathData, repository.Object, mapper.Object, container.Object);

            // Act
            var data = resolver.ResolvePath(virtualUrl);

            // Assert
            Assert.NotNull(data);
            Assert.AreEqual("myaction", data.Action);
            Assert.AreEqual("Content", data.Controller);
        }
    }

    [PageModel(Name = "Dummy", ControllerType = typeof(DummyController))]
    public class DummyModel : PageModel {}
    public class DummyController : Controller {}
}
