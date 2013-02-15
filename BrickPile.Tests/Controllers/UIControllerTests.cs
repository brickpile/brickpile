using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Tests.Common;
using Moq;
using NUnit.Framework;

namespace BrickPile.Tests.Controllers {
    //class UIControllerTests {
    //    [Test]
    //    public void Default_Action_Returns_Index_View() {
    //        // Arrange
    //        var request = new Mock<HttpRequestBase>();
    //        request.SetupGet(x => x.Headers).Returns(
    //            new System.Net.WebHeaderCollection
    //            {
    //                {"X-Requested-With", "XMLHttpRequest"}
    //            });
    //        const string expectedViewName = "Index";
    //        var context = new Mock<HttpContextBase>();
    //        context.SetupGet(x => x.Request).Returns(request.Object);

    //        var controller = new UIController();
    //        controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

    //        // Act
    //        var result = controller.Index() as PartialViewResult;

    //        // Assert
    //        Assert.IsNotNull(result, "Should have returned a ViewResult");
    //        Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
    //        //Assert.IsInstanceOf(typeof(DashboardViewModel), result.Model);
    //    }
    //}
}
