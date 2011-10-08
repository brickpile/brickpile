using System.Web.Mvc;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.UI;
using BrickPile.UI.Controllers;
using BrickPile.UI.Web.ViewModels;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace BrickPile.Tests.Controllers {
    public class ContentControllerTests {
        [Test]
        public void Default_Action_Returns_Index_View() {
            // Arrange
            const string expectedViewName = "Index";

            var model = new Mock<IPageModel>();
            var pageRepository = new Mock<IPageRepository>();
            var structureInfo = new Mock<IStructureInfo>();
            var documentSession = new Mock<IDocumentSession>();

            pageRepository.Setup(x => x.SingleOrDefault<IPageModel>(page => page.Parent == null)).Returns(model.Object);

            var controller = new ContentController(model.Object, structureInfo.Object, pageRepository.Object, documentSession.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
            Assert.IsInstanceOf(typeof(DashboardViewModel),result.Model);
        }
    }
}