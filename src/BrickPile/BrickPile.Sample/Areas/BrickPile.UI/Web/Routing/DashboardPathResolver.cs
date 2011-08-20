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

using System.Web;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using StructureMap;

namespace BrickPile.UI.Web.Routing {
    public class DashboardPathResolver : IPathResolver {
        private readonly IPathData _pathData;
        private IPageRepository _repository;
        private IPageModel _pageModel;

        public IPathData ResolvePath(string virtualUrl) {

            // Set the default action to index
            _pathData.Action = PageRoute.DefaultAction;
            // Get an up to date page repository
            _repository = ObjectFactory.GetInstance<IPageRepository>();


            if (string.IsNullOrEmpty(virtualUrl)) {
                _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
                _pathData.CurrentPageModel = _pageModel;
                _pathData.Controller = _pageModel.GetControllerName();
            } else {
                virtualUrl = VirtualPathUtility.RemoveTrailingSlash(virtualUrl);
                // The normal beahaviour should be to load the page based on the url
                _pageModel = _repository.GetPageByUrl<IPageModel>(virtualUrl);

                // Try to load the page without the last segment of the url and set the last segment as action
                if (_pageModel == null && virtualUrl.LastIndexOf("/") > 0) {
                    var index = virtualUrl.LastIndexOf("/");
                    var path = virtualUrl.Substring(0, index).TrimStart(new[] { '/' });
                    _pageModel = _repository.GetPageByUrl<IPageModel>(path);
                    _pathData.Action = virtualUrl.Substring(index, virtualUrl.Length - index).Trim(new[] { '/' });
                }

                if (_pageModel == null) {
                    _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
                    _pathData.CurrentPageModel = _pageModel;
                    _pathData.Action = virtualUrl;
                    _pathData.Controller = _pageModel.GetControllerName();
                }
            }

            if (_pageModel == null) {
                return null;
            }

            _pathData.CurrentPageModel = _pageModel;
            _pathData.Controller = _pathData.CurrentPageModel.GetControllerName();
            return _pathData;

            //// Set the default action to index
            //_pathData.Action = PageRoute.DefaultAction;
            //// Get an up to date page repository
            //_repository = ObjectFactory.GetInstance<IPageRepository>();

            //// Load and return the start page if the path is empty
            //if (string.IsNullOrEmpty(virtualUrl)) {
            //    _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
            //    if (_pageModel == null) {
            //        return null;
            //    }
            //    _pathData.CurrentPageModel = _pageModel;
            //    _pathData.Controller = _pageModel.GetControllerName();
            //    return _pathData;
            //}

            //// The normal beahaviour should be to load the page based on the url
            //_pageModel = _repository.GetPageByUrl<IPageModel>(virtualUrl);
            //// Try to load the page without the last segment of the url and set the last segment as action))
            //if (_pageModel == null) {
            //    var index = virtualUrl.LastIndexOf("/");
            //    // The url does not contain any action or the action is the url
            //    if (index < 1) {
            //        _pageModel = _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
            //        _pathData.Action = virtualUrl.Trim(new[] { '/' });
            //    } else {
            //        var path = virtualUrl.Substring(0, index).TrimStart(new[] { '/' });
            //        _pageModel = _repository.GetPageByUrl<IPageModel>(path);
            //        _pathData.Action = virtualUrl.Substring(index, virtualUrl.Length - index).Trim(new[] { '/' });
            //    }
            //}
            //if (_pageModel == null) {
            //    return null;
            //}

            //_pathData.CurrentPageModel = _pageModel;
            //_pathData.Controller = _pathData.CurrentPageModel.GetControllerName();
            //return _pathData;
        }

        public DashboardPathResolver(IPathData pathData, IPageRepository repository) {
            _pathData = pathData;
            _repository = repository;
        }
    }
}