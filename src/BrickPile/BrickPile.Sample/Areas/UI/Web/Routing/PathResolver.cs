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

using System.Linq;
using System.Web;
using System.Web.Routing;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using BrickPile.UI.Controllers;
using BrickPile.UI.Web.Mvc;
using Raven.Client;

namespace BrickPile.UI.Web.Routing {
    /// <summary>
    /// 
    /// </summary>
    public class PathResolver : IPathResolver {

        private readonly IPathData _pathData;
        private readonly IControllerMapper _controllerMapper;
        private IDocumentSession _session;
        private IPageModel _pageModel;
        private string _controllerName;
        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="virtualUrl">The virtual URL.</param>
        /// <returns></returns>
        public IPathData ResolvePath(RouteData routeData, string virtualUrl) {

            // Set the default action to index
            _pathData.Action = UIRoute.DefaultAction;
            // Get an up to date document session from structuremap
            _session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();

            // The requested url is for the start page with no action
            if (string.IsNullOrEmpty(virtualUrl) || string.Equals(virtualUrl, "/")) {
                _pageModel = _session.Query<IPageModel>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                    .SingleOrDefault(x => x.Parent == null);
            } else {
                // Remove the trailing slash
                virtualUrl = VirtualPathUtility.RemoveTrailingSlash(virtualUrl).TrimStart(new[] { '/' });
                // The normal beahaviour should be to load the page based on the url
                _pageModel = _session.Query<IPageModel, PageByUrl>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                    .FirstOrDefault(x => x.Metadata.Url == virtualUrl);
                // Try to load the page without the last segment of the url and set the last segment as action))
                if (_pageModel == null && virtualUrl.LastIndexOf("/") > 0) {
                    var index = virtualUrl.LastIndexOf("/");
                    var action = virtualUrl.Substring(index, virtualUrl.Length - index).Trim(new[] { '/' });
                    virtualUrl = virtualUrl.Substring(0, index).TrimStart(new[] { '/' });
                    _pageModel = _session.Query<IPageModel, PageByUrl>()
                        .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                        .FirstOrDefault(x => x.Metadata.Url == virtualUrl);
                    _pathData.Action = action;
                }
                // If the page model still is empty, let's try to resolve if the start page has an action named (virtualUrl)
                if (_pageModel == null) {
                    _pageModel = _session.Query<IPageModel>().SingleOrDefault(x => x.Parent == null);
                    if(_pageModel == null) {
                        return null;
                    }
                    var pageModelAttribute = _pageModel.GetType().GetAttribute<PageModelAttribute>();
                    object area;
                    _controllerName = _controllerMapper.GetControllerName(routeData.Values.TryGetValue("area", out area) ? typeof(PagesController) : pageModelAttribute.ControllerType);
                    var action = virtualUrl.TrimStart(new[] { '/' });
                    if (!_controllerMapper.ControllerHasAction(_controllerName, action)) {
                        return null;
                    }
                    _pathData.Action = action;
                }
            }

            if (_pageModel == null) {
                return null;
            }

            var controllerType = _pageModel.GetType().GetAttribute<PageModelAttribute>().ControllerType;
            _pathData.Controller = _controllerMapper.GetControllerName(controllerType);
            _pathData.CurrentPageModel = _pageModel;
            return _pathData;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathResolver"/> class.
        /// </summary>
        /// <param name="pathData">The path data.</param>
        /// <param name="controllerMapper">The controller mapper.</param>
        /// <param name="session">The session.</param>
        public PathResolver(IDocumentSession session, IPathData pathData, IControllerMapper controllerMapper) {
            _pathData = pathData;
            _controllerMapper = controllerMapper;
            _session = session;
        }
    }
}