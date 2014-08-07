using System.Web;
using System.Web.Routing;
using BrickPile.Core;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing;
using BrickPile.Tests.Fakes;
using FakeItEasy;
using Raven.Client;
using Raven.Tests.Helpers;
using Xunit;
using Xunit.Extensions;

namespace BrickPile.Tests.Routing
{
    public class PageRouteTests
    {
        public class GetRouteData : RavenTestBase
        {
            [Theory]
            [InlineData("")]
            [InlineData("/")]
            public void Can_Resolve_Home_Page_With_Default_Action(string path)
            {
                // Given

                var store = NewDocumentStore();

                var mapper = A.Fake<IControllerMapper>();
                var context = A.Fake<HttpContextBase>();

                A.CallTo(() => context.Request.Path).Returns(path);
                A.CallTo(() => mapper.GetControllerName(typeof (FakeController))).Returns("FakeController");                
                A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(true);

                // When

                RouteData data;

                using (var session = store.OpenSession())
                {
                    var node = new StructureInfo.Node
                    {
                        PageId = "pages/1"
                    };

                    var page = new FakePage { Id = "pages/1" };
                    var structureInfo = new StructureInfo
                    {
                        Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
                        RootNode = node
                    };
                    session.Store(structureInfo);
                    session.Store(page);
                    session.SaveChanges();

                    var route = new DefaultRoute(new VirtualPathResolver(), new RouteResolver(), () => store, mapper);
                    data = route.GetRouteData(context);
                }

                //Then

                Assert.NotNull(data);
                Assert.Equal("index", data.Values["action"]);
                Assert.Equal("FakeController", data.Values["controller"]);
            }

            [Theory]
            [InlineData("/myaction")]
            [InlineData("/myaction/")]
            public void Can_Resolve_Home_Page_With_Custom_Action(string path)
            {
                // Given

                var store = NewDocumentStore();
                var mapper = A.Fake<IControllerMapper>();
                var context = A.Fake<HttpContextBase>();

                A.CallTo(() => context.Request.Path).Returns(path);                
                A.CallTo(() => mapper.GetControllerName(typeof(FakeController))).Returns("FakeController");
                A.CallTo(() => mapper.ControllerHasAction("FakeController", "myaction")).Returns(true);

                // When

                RouteData data;

                using (var session = store.OpenSession())
                {
                    var node = new StructureInfo.Node
                    {
                        PageId = "pages/1"
                    };

                    var page = new FakePage { Id = "pages/1" };
                    session.Store(new StructureInfo
                    {
                        Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
                        RootNode = node
                    });
                    session.Store(page);
                    session.SaveChanges();

                    var route = new DefaultRoute(new VirtualPathResolver(), new RouteResolver(), () => store, mapper);
                    data = route.GetRouteData(context);
                }

                //Then

                Assert.NotNull(data);
                Assert.Equal("myaction", data.Values["action"]);
                Assert.Equal("FakeController", data.Values["controller"]);
            }

            [Theory]
            [InlineData("/page/myaction")]
            [InlineData("/page/myaction/")]
            public void Can_Resolve_Page_With_Custom_Action(string path)
            {
                // Given

                var store = NewDocumentStore();
                var mapper = A.Fake<IControllerMapper>();
                var context = A.Fake<HttpContextBase>();

                A.CallTo(() => context.Request.Path).Returns(path);
                A.CallTo(() => mapper.GetControllerName(typeof(FakeController))).Returns("FakeController");
                A.CallTo(() => mapper.ControllerHasAction("FakeController", "myaction")).Returns(true);

                // When

                RouteData data;

                using (var session = store.OpenSession())
                {
                    var node = new StructureInfo.Node
                    {
                        PageId = "pages/1",
                        Url = "page"
                    };

                    var page = new FakePage { Id = "pages/1", Metadata = { Url = "page" } };
                    session.Store(new StructureInfo
                    {
                        Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
                        RootNode = node
                    });
                    session.Store(page);
                    session.SaveChanges();

                    var route = new DefaultRoute(new VirtualPathResolver(), new RouteResolver(), () => store, mapper);
                    data = route.GetRouteData(context);
                }

                //Then

                Assert.NotNull(data);
                Assert.Equal("myaction", data.Values["action"]);
                Assert.Equal("FakeController", data.Values["controller"]);
            }

            [Theory]
            [InlineData("/content/js/modern-business.js")]
            public void Returns_Null_If_Request_Is_For_Static_Content(string path)
            {
                // Given

                var store = NewDocumentStore();
                var mapper = A.Fake<IControllerMapper>();
                var context = A.Fake<HttpContextBase>();

                A.CallTo(() => context.Request.Path).Returns(path);
                A.CallTo(() => mapper.GetControllerName(typeof(FakeController))).Returns("FakeController");
                A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(false);

                // When

                RouteData data;

                using (var session = store.OpenSession())
                {
                    var page = new FakePage();
                    session.Store(page);
                    var node = new StructureInfo.Node
                    {
                        PageId = page.Id
                    };
                    session.Store(new StructureInfo
                    {
                        Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
                        RootNode = node
                    });
                    session.SaveChanges();

                    var route = new DefaultRoute(new VirtualPathResolver(), new RouteResolver(), () => store, mapper);
                    data = route.GetRouteData(context);
                }

                // Then

                Assert.Null(data);
            }
            
        }
        public class GetVirtualPath
        {
            [Fact]
            public void Can_Resolve_Virtual_Path_For_Page()
            {
                // Given

                var store = A.Fake<IDocumentStore>();
                var mapper = A.Fake<IControllerMapper>();
                var context = A.Fake<HttpContextBase>();

                var currentPage = new FakePage
                {
                    Metadata =
                    {
                        Url = "fake-page"
                    }
                };
                context.Request.RequestContext.RouteData.Values[DefaultRoute.CurrentPageKey] = currentPage;

                // When

                var route = new DefaultRoute(new VirtualPathResolver(), new RouteResolver(), () => store, mapper);
                VirtualPathData data = route.GetVirtualPath(context.Request.RequestContext, new RouteValueDictionary(new { currentPage }));

                // Then

                Assert.NotNull(data);
                Assert.Equal("fake-page/", data.VirtualPath);
            }

            [Fact]
            public void Can_Resolve_Virtual_Path_For_Page_With_Query_Parameter()
            {
                // Given

                var store = A.Fake<IDocumentStore>();
                var mapper = A.Fake<IControllerMapper>();
                var context = A.Fake<HttpContextBase>();

                var currentPage = new FakePage
                {
                    Metadata =
                    {
                        Url = "fake-page"
                    }
                };
                context.Request.RequestContext.RouteData.Values[DefaultRoute.CurrentPageKey] = currentPage;

                // When

                var route = new DefaultRoute(new VirtualPathResolver(), new RouteResolver(), () => store, mapper);
                VirtualPathData data = route.GetVirtualPath(context.Request.RequestContext, new RouteValueDictionary(new { currentPage, page = "1", mode = "edit" }));

                // Then

                Assert.NotNull(data);
                Assert.Equal("fake-page/?page=1&mode=edit", data.VirtualPath);
            }
        }
    }
}
