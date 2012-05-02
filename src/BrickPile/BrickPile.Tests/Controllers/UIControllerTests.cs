using System.Web.Mvc;
using BrickPile.UI.Controllers;
using NUnit.Framework;

namespace BrickPile.Tests.Controllers {
    class UIControllerTests {
        //[Test]
        public void Default_Action_Returns_Index_View() {
            // Arrange
            const string expectedViewName = "Index";

            var controller = new UIController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
            //Assert.IsInstanceOf(typeof(DashboardViewModel), result.Model);
        }
    }
}
