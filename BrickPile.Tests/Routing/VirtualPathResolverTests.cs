using System.Web.Routing;
using BrickPile.Core;
using BrickPile.Core.Routing;
using BrickPile.Tests.Fakes;
using Xunit;

namespace BrickPile.Tests.Routing
{
    public class VirtualPathResolverTests
    {
        public class ResolveVirtualPath
        {
            [Fact]
            public void Can_Resolve_Virtual_Path()
            {
                // Given
                var page = new FakePage {Metadata = { Url = "foo/bar/fudge"}};
                var resolver = new VirtualPathResolver();

                // When

                var result = resolver.ResolveVirtualPath(page, new RouteValueDictionary( new { action = "index" } ));

                // Then

                Assert.Equal("foo/bar/fudge/", result);
            }

            [Fact]
            public void Can_Append_Action_To_Url()
            {
                // Given
                var page = new FakePage { Metadata = { Url = "foo/bar/fudge" } };
                var resolver = new VirtualPathResolver();

                // When

                var result = resolver.ResolveVirtualPath(page, new RouteValueDictionary(new { action = "comment" }));

                // Then

                Assert.Equal("foo/bar/fudge/comment/", result);
            }
        }
    }
}
