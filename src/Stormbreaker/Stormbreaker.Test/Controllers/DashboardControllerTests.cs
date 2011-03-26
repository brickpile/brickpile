using Dashboard.Controllers;
using NUnit.Framework;

namespace Stormbreaker.Test.Controllers {
    public class DashboardControllerTests {
        [Test]
        public void Test_Index() {
            //Arrange
            var controller = new DashboardController();
            //Act
            var result = controller.Index();
            //Assert
            Assert.NotNull(result);
        }
    }
}