/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.UI.Web.Routing;

namespace BrickPile.UI.Common {
    /// <summary>
    /// 
    /// </summary>
    public static class RouteCollectionExtensions {
        /// <summary>
        /// Maps the UI route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL as it may contain special routing characters.")]
        public static Route MapUIRoute(this RouteCollection routes, string name, string url) {
            return MapUIRoute(routes, name, url, null /* defaults */, (object)null /* constraints */);
        }
        /// <summary>
        /// Maps the UI route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL as it may contain special routing characters.")]
        public static Route MapUIRoute(this RouteCollection routes, string name, string url, object defaults) {
            return MapUIRoute(routes, name, url, defaults, (object)null /* constraints */);
        }
        /// <summary>
        /// Maps the UI route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL as it may contain special routing characters.")]
        public static Route MapUIRoute(this RouteCollection routes, string name, string url, object defaults, object constraints) {
            return MapUIRoute(routes, name, url, defaults, constraints, null /* namespaces */);
        }
        /// <summary>
        /// Maps the UI route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL as it may contain special routing characters.")]
        public static Route MapUIRoute(this RouteCollection routes, string name, string url, string[] namespaces) {
            return MapUIRoute(routes, name, url, null /* defaults */, null /* constraints */, namespaces);
        }
        /// <summary>
        /// Maps the UI route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL as it may contain special routing characters.")]
        public static Route MapUIRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces) {
            return MapUIRoute(routes, name, url, defaults, null /* constraints */, namespaces);
        }
        /// <summary>
        /// Maps the UI route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL as it may contain special routing characters.")]
        public static Route MapUIRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces) {
            if (routes == null) {
                throw new ArgumentNullException("routes");
            }
            if (url == null) {
                throw new ArgumentNullException("url");
            }

            Route route = new UIRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };

            if ((namespaces != null) && (namespaces.Length > 0)) {
                route.DataTokens = new RouteValueDictionary();
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }     
    }
}