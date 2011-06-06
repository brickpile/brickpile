using System;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Stormbreaker.Dashboard.Controllers;
using Stormbreaker.Dashboard.Web.Mvc.ViewModels;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Test.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Test.Controllers {
    public class ContentControllerTests {
        [Test]
        public void IndexTest() {
            //Arrange
            var model = new DummyPageModel { Parent = null };
            var pageRepository = new Mock<IPageRepository>();
            var structureInfo = new Mock<IStructureInfo>();

            pageRepository.Setup(x => x.SingleOrDefault(It.Is<Func<IPageModel, bool>>(page => model.Parent == null))).Returns(model);

            var controller = new ContentController(pageRepository.Object, structureInfo.Object);

            //Act
            //var result = controller.Index() as RedirectToRouteResult;

            //Assert
            //Assert.NotNull(result);
            //Assert.AreEqual("edit", result.RouteValues["action"]);
            //Assert.AreEqual(model, result.RouteValues["model"]);
        }
        [Test]
        public void Test_Edit() {
            //Arrange
            var model = new DummyPageModel();
            var pageRepository = new Mock<IPageRepository>();
            var structureInfo = new Mock<IStructureInfo>();

            var controller = new ContentController(pageRepository.Object, structureInfo.Object);
            //Act
            var result = controller.Edit(model) as ViewResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(DashboardViewModel),result.Model);
        }
        [Test]
        public void Test_Delete() {

            //Arrange
            var page = new DummyPageModel();
            var pageRepository = new Mock<IPageRepository>();
            pageRepository.Setup(x => x.Delete(page));
            //var structureInfo = new StructureInfo(pageRepository.Object);
            //var controller = new ContentController(pageRepository.Object, structureInfo);

            //Act
            //var result = controller.Delete(page) as RedirectToRouteResult;

            //Assert
            //Assert.NotNull(result);
            //Assert.AreEqual("index",result.RouteValues["action"]);

        }
    }
}