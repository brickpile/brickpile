using System.Web.Routing;
using BrickPile.UI.Common;
using BrickPile.UI.Web.Routing;
using Moq;
using NUnit.Framework;

namespace BrickPile.Tests.Web.Routing {
    public class PageRouteTests {
        [Test]
        public void Home_Page_With_Default_Action() {
            // Arrange
                        
            var pathData = new Mock<IPathData>();
            var pathResolver = new Mock<IPathResolver>();
            var virtialPathResolver = new Mock<IVirtualPathResolver>();
            var routes = RouteTable.Routes.RegisterPageRoute(pathResolver.Object, virtialPathResolver.Object);

            pathResolver.Setup(x => x.ResolvePath("~/")).Returns(pathData.Object);

            // Act
            var context = Common.MvcMockHelpers.FakeHttpContext("~/");
            var routeData = routes.GetRouteData(context);

            // Assert
            Assert.NotNull(routeData);
        }            
    }
}