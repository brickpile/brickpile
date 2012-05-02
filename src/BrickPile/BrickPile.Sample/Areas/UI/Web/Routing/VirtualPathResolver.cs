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
using System.Web.Routing;
using BrickPile.Core.Exception;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.Routing {
    public class VirtualPathResolver : IVirtualPathResolver {
        private string _action;
        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        public virtual string ResolveVirtualPath(IPageModel pageModel, RouteValueDictionary routeValueDictionary) {

            if (pageModel == null) {
                return null;
            }
            var url = pageModel.Metadata.Url ?? string.Empty;

            if (routeValueDictionary.ContainsKey(UIRoute.ActionKey)) {
                _action = routeValueDictionary[UIRoute.ActionKey] as string;
                if (_action != null && !_action.Equals(UIRoute.DefaultAction)) {
                    return string.Format("{0}/{1}/", url, _action);
                }
            }
            return string.Format("{0}", VirtualPathUtility.AppendTrailingSlash(url));
        }
    }
}