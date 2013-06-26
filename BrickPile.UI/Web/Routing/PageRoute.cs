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
using System.Linq;
using System.Web;
using System.Web.Routing;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using StructureMap;

namespace BrickPile.UI.Web.Routing {
    public class PageRoute : Route {
        private readonly string _url;
        private readonly RouteValueDictionary _defaults;
        private readonly RouteValueDictionary _constraints;
        private readonly RouteValueDictionary _dataTokens;
        private readonly IRouteHandler _routeHandler;
        /// <summary>
        /// The key for the controller
        /// </summary>
        public const string ControllerKey = "controller";
        /// <summary>
        /// The key for the structure info
        /// </summary>
        public const string StructureInfoKey = "structureInfo";
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
        /// Gets the path resolver.
        /// </summary>
        protected IPathResolver PathResolver {
            get { return _pathResolver ?? (_pathResolver = ObjectFactory.GetInstance<IPathResolver>()); }
        }
        private IPathResolver _pathResolver;
        /// <summary>
        /// Gets the virtual path resolver.
        /// </summary>
        protected IVirtualPathResolver VirtualPathResolver {
            get { return _virtualPathResolver ?? (_virtualPathResolver = ObjectFactory.GetInstance<IVirtualPathResolver>()); }
        }
        private IVirtualPathResolver _virtualPathResolver;
        /// <summary>
        /// Gets the model key.
        /// </summary>
        public static string ModelKey {
            get { return "currentPage"; }
        }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <param name="httpContextBase">The HTTP context base.</param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContextBase) {
            var routeData = new RouteData(this, _routeHandler);

            // get the virtual path of the request
            var virtualPath = httpContextBase.Request.CurrentExecutionFilePath.TrimStart(new[] { '/' });

            // try to resolve the current item
            var pathData = this.PathResolver.ResolvePath(routeData, virtualPath);

            // Abort and proceed to other routes in the route table
            if (pathData == null) {
                return null;
            }

            // throw a proper 404 if the page is not published or if it's deleted
            if((!pathData.CurrentPage.Metadata.IsPublished || pathData.CurrentPage.Metadata.IsDeleted) && !httpContextBase.User.Identity.IsAuthenticated ) {
                throw new HttpException(404, "HTTP/1.1 404 Not Found");
            }
            
            routeData.ApplyCurrentPage(pathData.Controller, pathData.Action, pathData.CurrentPage);
            routeData.ApplyCurrentStructureInfo(new StructureInfo
            {
                NavigationContext = httpContextBase.User.Identity.IsAuthenticated ? pathData.NavigationContext.Where(x => !x.Metadata.IsDeleted).OrderBy(x => x.Metadata.SortOrder) : pathData.NavigationContext.Where(x => x.Metadata.IsPublished).Where(x => !x.Metadata.IsDeleted).OrderBy(x => x.Metadata.SortOrder),
                CurrentPage = pathData.CurrentPage,
                StartPage = pathData.NavigationContext.Single(x => x.Parent == null),
                ParentPage = pathData.CurrentPage.Parent != null ? pathData.NavigationContext.SingleOrDefault(x => x.Id == pathData.CurrentPage.Parent.Id) : null
            });

            return routeData;
        }
        /// <summary>
        /// Returns information about the URL that is associated with the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains information about the URL that is associated with the route.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {

            var model = values[ModelKey] as IPageModel ??
                requestContext.RouteData.GetCurrentPage<IPageModel>();

            if (model == null) {
                return null;
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.ResolveVirtualPath(model, values));

            var queryParams = String.Empty;
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
        /// <param name="url">The URL.</param>
        /// <param name="routeHandler">The route handler.</param>
        public PageRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) {
            _url = url;
            _routeHandler = routeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="routeHandler">The route handler.</param>
        public PageRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler) {
            _url = url;
            _defaults = defaults;
            _routeHandler = routeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="routeHandler">The route handler.</param>
        public PageRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler) {
            _url = url;
            _defaults = defaults;
            _constraints = constraints;
            _routeHandler = routeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRoute"/> class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. These values are passed to the route handler, where they can be used for processing the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public PageRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) {
            _url = url;
            _defaults = defaults;
            _constraints = constraints;
            _dataTokens = dataTokens;
            _routeHandler = routeHandler;
        }
    }
}