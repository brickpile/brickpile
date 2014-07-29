using System.Web;
using System.Web.Routing;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing;
using BrickPile.Tests.Fakes;
using FakeItEasy;
using Raven.Client;
using Xunit;

namespace BrickPile.Tests.Routing
{
    public class PageRouteTests
    {
        //public class GetRouteData {

        //    [Fact]
        //    public void Returns_Null_When_Page_Not_Found() {
        //        // Given
        //        var context = A.Fake<HttpContextBase>();
        //        var resolver = A.Fake<IRouteResolver>();

        //        A.CallTo(() => resolver.ResolvePath(context, "")).Returns(null);

        //        // When
        //        var route = new PageRoute(new VirtualPathResolver(), resolver);
        //        var result = route.GetRouteData(context);

        //        // Then
        //        Assert.Null(result);
        //    }

        //    [Fact]
        //    public void Throws_HttpException_When_Page_Not_Published() {
        //        // Given
        //        var context = A.Fake<HttpContextBase>();
        //        var resolver = A.Fake<IRouteResolver>();

        //        A.CallTo(
        //            () => resolver.ResolvePath(context, ""))
        //            .Returns(
        //                new PathData
        //                {
        //                    CurrentPage = new FakePage
        //                    {
        //                        Metadata =
        //                        {
        //                            Published = new DateTime(2015, 1, 1)
        //                        }
        //                    }
        //                });

        //        // When
        //        var route = new PageRoute(new VirtualPathResolver(), resolver);

        //        Assert.Throws(typeof (HttpException), () => route.GetRouteData(context));

        //    }

        //    [Fact]
        //    public void Returns_UnPublished_Page_When_User_Is_Authenticated()
        //    {
        //        // Given
        //        var context = A.Fake<HttpContextBase>();
        //        var resolver = A.Fake<IRouteResolver>();

        //        A.CallTo(() => context.User.Identity.IsAuthenticated).Returns(true);
        //        A.CallTo(
        //            () => resolver.ResolvePath(context, ""))
        //            .Returns(
        //                new PathData
        //                {
        //                    CurrentPage = new FakePage
        //                    {
        //                        Metadata =
        //                        {
        //                            Published = new DateTime(2015, 1, 1)
        //                        }
        //                    }
        //                });

        //        // When
        //        var route = new PageRoute(new VirtualPathResolver(), resolver);
        //        var result = route.GetRouteData(context);
                
        //        // Then

        //        Assert.NotNull(result);

        //    }

        //}

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
                context.Request.RequestContext.RouteData.Values[PageRoute.CurrentPageKey] = currentPage;

                // When

                var route = new PageRoute(new VirtualPathResolver(), new RouteResolver(() => store, mapper));
                VirtualPathData data = route.GetVirtualPath(context.Request.RequestContext, new RouteValueDictionary(new {currentPage }));

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
                context.Request.RequestContext.RouteData.Values[PageRoute.CurrentPageKey] = currentPage;

                // When

                var route = new PageRoute(new VirtualPathResolver(), new RouteResolver(() => store, mapper));
                VirtualPathData data = route.GetVirtualPath(context.Request.RequestContext, new RouteValueDictionary(new {currentPage, page = "1", mode = "edit" }));

                // Then

                Assert.NotNull(data);
                Assert.Equal("fake-page/?page=1&mode=edit", data.VirtualPath);
            }
        }
    }
}
