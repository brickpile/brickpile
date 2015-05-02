using System.Linq;
using BrickPile.Core;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Tests.Fakes;
using Raven.Client.Indexes;
using Raven.Tests.Helpers;
using Xunit;
using IPage = BrickPile.Core.IPage;

namespace BrickPile.Tests.Infrastructure.Indexes
{
    public class AllPagesTests : RavenTestBase
    {
        [Fact]
        public void Can_Query_Page_Using_AllPages_Index()
        {

            // Arrange
            IPage data;

            using (var store = NewDocumentStore())
            {

                IndexCreation.CreateIndexes(typeof (DefaultBrickPileBootstrapper).Assembly, store);

                // Act
                using (var session = store.OpenSession())
                {

                    var pageModel = new FakePage
                    {
                        Id = "fakepages/1",
                        Parent = null,
                        Metadata = {Name = "Foo"}
                    };
                    session.Store(pageModel);
                    session.SaveChanges();

                }
                using (var session = store.OpenSession())
                {
                    data = session.Query<IPage, AllPages>()
                        .Customize(x => x.WaitForNonStaleResults())
                        .SingleOrDefault(x => x.Metadata.Name == "Foo");
                }

                // Assert
                Assert.NotNull(data);
            }
        }
    }
}
