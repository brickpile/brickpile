using System;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using BrickPile.Core;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing;
using BrickPile.Core.Routing.Trie;
using BrickPile.Tests.Fakes;
using FakeItEasy;
using Raven.Client;
using Raven.Tests.Helpers;
using Xunit;
using Xunit.Extensions;

namespace BrickPile.Tests.Routing
{
    public class UrlResolverTests {
        public class ResolvePath : RavenTestBase {

            private IDocumentStore SetupContext()
            {

                HttpContext.Current = null;
                RouteTable.Routes.Clear();

                var store = NewDocumentStore();

                var bootStrapper = new FakeBootstrapper();
                var field = typeof(DefaultBrickPileBootstrapper).GetField("DocStore",
                    BindingFlags.Static | BindingFlags.NonPublic);

                field.SetValue(null, new Lazy<IDocumentStore>(() => store));
                bootStrapper.Initialise();

                HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null))
                {
                    User = A.Fake<IPrincipal>()
                };
                return store;
            }

            [Theory]
            [InlineData("")]
            [InlineData("/")]
            public void Can_Resolve_Home_Page_With_Default_Action(string path) {
                // Given

                var store = this.SetupContext();
                var routeResolverTrie = A.Fake<IRouteResolverTrie>();
                //var mapper = A.Fake<IControllerMapper>();


                
                //A.CallTo(() => mapper.GetControllerName(typeof (FakeController))).Returns("FakeController");                
                //A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(true);

                // When

                ResolveResult data;

                using (var session = store.OpenSession()) {
                    var node = new TrieNode
                    {
                        PageId = "pages/1"
                    };

                    var page = new FakePage {Id = "pages/1"};
                    var structureInfo = new Trie
                    {
                        Id = DefaultBrickPileBootstrapper.TrieId,
                        RootNode = node
                    };
                    session.Store(structureInfo);
                    session.Store(page);
                    session.SaveChanges();

                    var resolver = new DefaultRouteResolver(routeResolverTrie);

                    A.CallTo(() => routeResolverTrie.LoadTrie()).Returns(structureInfo);

                    data = resolver.Resolve(path);
                }

                //Then

                Assert.NotNull(data);
                Assert.Equal(data.TrieNode.PageId, "pages/1");
                Assert.Equal("index", data.Action);
            }

            //[Theory]
            //[InlineData("/myaction")]
            //[InlineData("/myaction/")]
            //public void Can_Resolve_Home_Page_With_Custom_Action(string path)
            //{
            //    // Given

            //    var store = NewDocumentStore();
            //    var mapper = A.Fake<IControllerMapper>();
            //    var context = A.Fake<HttpContextBase>();
            //    var route = A.Fake<PageRoute>();

            //    A.CallTo(() => mapper.GetControllerName(typeof(FakeController))).Returns("FakeController");
            //    A.CallTo(() => mapper.ControllerHasAction("FakeController", "myaction")).Returns(true);

            //    // When

            //    RouteData data;

            //    using (var session = store.OpenSession())
            //    {
            //        var node = new StructureInfo.Node
            //        {
            //            PageId = "pages/1"
            //        };

            //        var page = new FakePage { Id = "pages/1" };
            //        session.Store(new StructureInfo
            //        {
            //            Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
            //            RootNode = node
            //        });
            //        session.Store(page);
            //        session.SaveChanges();

            //        var resolver = new RouteResolver(() => store, mapper);
            //        data = resolver.ResolveRoute(route, context, path);
            //    }

            //    //Then

            //    Assert.NotNull(data);
            //    Assert.Equal("myaction", data.Values["action"]);
            //    Assert.Equal("FakeController", data.Values["controller"]);
            //}

        //    [Theory]
        //    [InlineData("/page/myaction")]
        //    [InlineData("/page/myaction/")]
        //    public void Can_Resolve_Page_With_Custom_Action(string path) {
        //        // Given

        //        var store = NewDocumentStore();
        //        var mapper = A.Fake<IControllerMapper>();
        //        var context = A.Fake<HttpContextBase>();
        //        var route = A.Fake<PageRoute>();

        //        A.CallTo(() => mapper.GetControllerName(typeof (FakeController))).Returns("FakeController");
        //        A.CallTo(() => mapper.ControllerHasAction("FakeController", "myaction")).Returns(true);

        //        // When

        //        RouteData data;

        //        using (var session = store.OpenSession()) {
        //            var node = new StructureInfo.Node
        //            {
        //                PageId = "pages/1",
        //                Url = "page"
        //            };

        //            var page = new FakePage {Id = "pages/1", Metadata = {Url = "page"}};
        //            session.Store(new StructureInfo
        //            {
        //                Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
        //                RootNode = node
        //            });
        //            session.Store(page);
        //            session.SaveChanges();

        //            var resolver = new RouteResolver(() => store, mapper);
        //            data = resolver.ResolveRoute(route, context, path);
        //        }

        //        //Then

        //        Assert.NotNull(data);
        //        Assert.Equal("myaction", data.Values["action"]);
        //        Assert.Equal("FakeController", data.Values["controller"]);
        //    }

        //    [Theory]
        //    [InlineData("/page")]
        //    [InlineData("/page/")]
        //    public void Can_Resolve_Page_Draft_With_Default_Action(string path) {
        //        // Given

        //        var store = NewDocumentStore();
        //        var mapper = A.Fake<IControllerMapper>();
        //        var context = A.Fake<HttpContextBase>();
        //        var route = A.Fake<PageRoute>();

        //        A.CallTo(() => mapper.GetControllerName(typeof (FakeController))).Returns("FakeController");
        //        A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(true);
        //        A.CallTo(() => context.User.Identity.IsAuthenticated).Returns(true);

        //        // When

        //        RouteData data;

        //        using (var session = store.OpenSession()) {
        //            var node = new StructureInfo.Node
        //            {
        //                PageId = "pages/1",
        //                Url = "page"
        //            };

        //            var page = new FakePage {Id = "pages/1", Metadata = {Slug = "page", Url = "page"}};
        //            session.Store(new StructureInfo
        //            {
        //                Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
        //                RootNode = node
        //            });
        //            session.Store(page);
        //            session.SaveChanges();

        //            var resolver = new RouteResolver(() => store, mapper);
        //            data = resolver.ResolveRoute(route, context, path);
        //        }

        //        //Then

        //        Assert.NotNull(data);
        //        Assert.Equal("index", data.Values["action"]);
        //        Assert.Equal("FakeController", data.Values["controller"]);
        //    }

        //    [Theory]
        //    [InlineData("/content/js/modern-business.js")]
        //    public void Returns_Null_If_Request_Is_For_Static_Content(string path) {
        //        // Given

        //        var store = NewDocumentStore();
        //        var mapper = A.Fake<IControllerMapper>();
        //        var context = A.Fake<HttpContextBase>();
        //        var route = A.Fake<PageRoute>();

        //        A.CallTo(() => mapper.GetControllerName(typeof (FakeController))).Returns("FakeController");
        //        A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(false);

        //        // When

        //        RouteData data;

        //        using (var session = store.OpenSession()) {
        //            var page = new FakePage();
        //            session.Store(page);
        //            var node = new StructureInfo.Node
        //            {
        //                PageId = page.Id
        //            };
        //            session.Store(new StructureInfo
        //            {
        //                Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
        //                RootNode = node
        //            });
        //            session.SaveChanges();

        //            var resolver = new RouteResolver(() => store, mapper);
        //            data = resolver.ResolveRoute(route, context, path);
        //        }

        //        // Then

        //        Assert.Null(data);
        //    }

        //    [Theory(Skip = "Skipped due to code move")]
        //    [InlineData("/page")]
        //    [InlineData("/page/")]
        //    public void Returns_Null_When_User_Is_Not_Authenticated(string path) {
        //        // Given

        //        var store = NewDocumentStore();
        //        var mapper = A.Fake<IControllerMapper>();
        //        var context = A.Fake<HttpContextBase>();
        //        var route = A.Fake<PageRoute>();

        //        A.CallTo(() => mapper.GetControllerName(typeof(FakeController))).Returns("FakeController");                
        //        A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(true);

        //        // When

        //        RouteData data;

        //        using (var session = store.OpenSession())
        //        {
        //            var node = new StructureInfo.Node
        //            {
        //                Url = "page"
        //            };

        //            var page = new FakePage { Id = "pages/1/draft", Metadata = { Slug = "page", Url = "page" } };

        //            session.Store(new StructureInfo
        //            {
        //                Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
        //                RootNode = node
        //            });

        //            session.Store(page);
        //            session.SaveChanges();

        //            var resolver = new RouteResolver(() => store, mapper);

        //            data = resolver.ResolveRoute(route, context, path);
                    
        //        }

        //        //Then

        //        Assert.Null(data);
        //    }

        //    [Theory]
        //    [InlineData("/page")]
        //    [InlineData("/page/")]
        //    public void Returns_Draft_When_User_Is_Not_Authenticated(string path)
        //    {
        //        // Given

        //        var store = NewDocumentStore();
        //        var mapper = A.Fake<IControllerMapper>();
        //        var context = A.Fake<HttpContextBase>();
        //        var route = A.Fake<PageRoute>();

        //        A.CallTo(() => mapper.GetControllerName(typeof(FakeController))).Returns("FakeController");                
        //        A.CallTo(() => mapper.ControllerHasAction("FakeController", "index")).Returns(true);
        //        A.CallTo(() => context.User.Identity.IsAuthenticated).Returns(true);

        //        // When

        //        RouteData data;

        //        using (var session = store.OpenSession())
        //        {
        //            var node = new StructureInfo.Node
        //            {
        //                PageId = "pages/1",
        //                Url = "page"
        //            };

        //            var page = new FakePage { Id = "pages/1", Metadata = { Slug = "page", Url = "page" } };

        //            session.Store(new StructureInfo
        //            {
        //                Id = DefaultBrickPileBootstrapper.StructureInfoDocumentId,
        //                RootNode = node
        //            });

        //            session.Store(page);
        //            session.SaveChanges();

        //            var resolver = new RouteResolver(() => store, mapper);

        //            data = resolver.ResolveRoute(route, context, path);

        //        }

        //        //Then

        //        Assert.NotNull(data);
        //        Assert.Equal("index", data.Values["action"]);
        //        Assert.Equal("FakeController", data.Values["controller"]);                
        //    }
        }
    }
}
