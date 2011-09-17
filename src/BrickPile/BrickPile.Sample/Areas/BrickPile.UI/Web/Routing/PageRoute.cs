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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;

namespace BrickPile.UI.Web.Routing {
    /// <summary>
    /// Responsible for mapping all routes to the ContentRoute
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PageRoute : RouteBase {
        private readonly IPathResolver _pathResolver;
        private readonly IVirtualPathResolver _virtualPathResolver;
        private readonly IRouteHandler _routeHandler;
        private readonly Route _innerRoute;
        public const string ControllerKey = "controller";
        /// <summary>
        /// Gets the model key.
        /// </summary>
        public static string ModelKey {
            get { return "model"; }
        }
        /// <summary>
        /// Gets the action key.
        /// </summary>
        public static string ActionKey {
            get { return "action"; }
        }
        /// <summary>
        /// Gets the default action.
        /// </summary>
        public static string DefaultAction {
            get { return "index"; }
        }
        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <param name="httpContextBase">The HTTP context base.</param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContextBase) {

            if (httpContextBase.Request.CurrentExecutionFilePath.StartsWith("/dashboard", StringComparison.InvariantCultureIgnoreCase))
                return null;

            var routeData = new RouteData(this, _routeHandler);

            foreach (var defaultPair in _innerRoute.Defaults)
                routeData.Values[defaultPair.Key] = defaultPair.Value;
            foreach (var tokenPair in _innerRoute.DataTokens)
                routeData.DataTokens[tokenPair.Key] = tokenPair.Value;

            // get the virtual path of the request
            var virtualPath = httpContextBase.Request.CurrentExecutionFilePath.TrimStart(new[] {'/'});
            //var virtualPath = httpContextBase.Request.AppRelativeCurrentExecutionFilePath;
            
            // try to resolve the current item
            var pathData = _pathResolver.ResolvePath(virtualPath);

            // Abort and proceed to other routes in the route table
            if (pathData == null) {
                return null;
            }

            routeData.ApplyCurrentModel(pathData.Controller, pathData.Action, pathData.CurrentPageModel);

            return routeData;
        }
        /// <summary>
        /// When overridden in a derived class, checks whether the route matches the specified values, and if so, generates a URL and retrieves information about the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains the generated URL and information about the route, or null if the route does not match <paramref name="values"/>.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            var model = values[ModelKey] as IPageModel;

            if (model == null) {
                return null;
            }
            
            var vpd = _innerRoute.GetVirtualPath(requestContext, values);
            
            if (vpd == null) {
                return null;
            }

            vpd.Route = this;

            vpd.VirtualPath = _virtualPathResolver.ResolveVirtualPath(model, values);

            var queryParams = string.Empty;
            // add query string parameters
            foreach (var kvp in values) {
                if (kvp.Key.Equals(ModelKey) || kvp.Key.Equals(ControllerKey) || kvp.Key.Equals(ActionKey)) {
                    continue;
                }
                queryParams = queryParams.AddQueryParam(kvp.Key, kvp.Value.ToString());
            }
            vpd.VirtualPath += queryParams;
            return vpd;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRoute"/> class.
        /// </summary>
        /// <param name="pathResolver">The path resolver.</param>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        public PageRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) : this(pathResolver, virtualPathResolver, null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRoute"/> class.
        /// </summary>
        /// <param name="pathResolver">The path resolver.</param>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <param name="innerRoute">The inner route.</param>
        public PageRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver, Route innerRoute) {
            _pathResolver = pathResolver;
            _virtualPathResolver = virtualPathResolver;
            _routeHandler = _routeHandler ?? new MvcRouteHandler();
            _innerRoute = innerRoute ?? new Route("{controller}/{action}",
                                                        new RouteValueDictionary(new { action = "index" }),
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary(),
                                                        _routeHandler);
        }
    }
}