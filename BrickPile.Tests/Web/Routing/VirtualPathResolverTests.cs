﻿using System.Web.Routing;
using BrickPile.Domain.Models;
using BrickPile.UI.Web.Routing;
using NUnit.Framework;

namespace BrickPile.Tests.Web.Routing {

    /// <summary>
    /// Summary description for VirtualPathResolverTests.
    /// </summary>
    /// <remarks>
    /// 2013-03-16 marcus: Created
    /// </remarks>
    public class VirtualPathResolverTests {

        [Test]
        public void Can_Resolve_Virtual_Path_With_Default_Action() {

            // Arrange
            IPage model = new Page {Metadata = {Url = "page1/page2"}};
            var routeValueDictionary = new RouteValueDictionary();

            // Act

            var virtualPathResolver = new VirtualPathResolver();
            var virtualPath = virtualPathResolver.ResolveVirtualPath(model, routeValueDictionary);

            // Assert

            Assert.AreEqual("page1/page2/", virtualPath);
        }


        [Test]
        public void Can_Resolve_Virtual_Path_With_Comment_Action() {

            // Arrange
            IPage model = new Page { Metadata = { Url = "page1/page2" } };
            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary["Action"] = "Comment";

            // Act

            var virtualPathResolver = new VirtualPathResolver();
            var virtualPath = virtualPathResolver.ResolveVirtualPath(model, routeValueDictionary);

            // Assert

            Assert.AreEqual("page1/page2/comment/", virtualPath);
        }
    }
}