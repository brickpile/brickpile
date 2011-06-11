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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BrickPile.Domain.Models;
using BrickPile.UI.Web.Routing;

namespace BrickPile.UI.Common {
    public static class Extensions {
        /// <summary>
        /// Creates an hierarchical structure of <see cref="IPageModel"/> objects used in navigation scenarios.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="allItems">All items.</param>
        /// <param name="rootPage">Collection to create the hierarchy from</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        public static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity>(this IEnumerable<TEntity> allItems, TEntity rootPage, int depth) where TEntity : IPageModel {
            var childs = allItems.Where(x => x.Parent.Id.Equals(rootPage.Id));
            if (childs.Count() > 0) {
                childs.OrderByDescending(x => x.SortOrder);
                depth++;
                foreach (var item in childs)
                    yield return new HierarchyNode<TEntity>
                    {
                        Entity = item,
                        ChildNodes = CreateHierarchy(allItems, item, depth),
                        Depth = depth,
                        Expanded = allItems.Where(x => x.Parent.Id.Equals(item.Id)).Count() > 0
                    };
            }
        }
        /// <summary>
        /// Returns the controller name associated with this model
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public static string GetControllerName(this IPageModel pageModel) {
            return pageModel.GetType().Name.ToLower();
        }
        /// <summary>
        /// Registers the page route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="pathResolver">The path resolver.</param>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <returns></returns>
        internal static RouteCollection RegisterPageRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) {
            var pageRoute = new PageRoute(pathResolver, virtualPathResolver);
            routes.Add("PageRoute", pageRoute);
            return routes;
        }
        /// <summary>
        /// Used for adding a page model to the RouteData object's DataTokens
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static RouteData ApplyCurrentModel(this RouteData data, string controllerName, string actionName, dynamic model) {
            data.Values[PageRoute.ControllerKey] = controllerName;
            data.Values[PageRoute.ActionKey] = actionName;
            data.Values[PageRoute.ModelKey] = model;
            return data;
        }
        /// <summary>
        /// Returns the current model of the current request
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T GetCurrentModel<T>(this RouteData data) {
            return (T)data.Values[PageRoute.ModelKey];
        }
        /// <summary>
        /// Adds the query param.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string AddQueryParam(this string source, string key, string value) {
            string delim;
            if ((source == null) || !source.Contains("?")) {
                delim = "?";
            } else if (source.EndsWith("?") || source.EndsWith("&")) {
                delim = string.Empty;
            } else {
                delim = "&";
            }
            return source + delim + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value);
        }
        /// <summary>
        /// Formats the size of the file.
        /// </summary>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        public static string FormatFileSize(this long fileSize) {
            string[] suffix = { "bytes", "KB", "MB", "GB" };
            long j = 0;

            while (fileSize > 1024 && j < 4) {
                fileSize = fileSize / 1024;
                j++;
            }
            return (fileSize + " " + suffix[j]);
        }
        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IPageModel model) {
            return htmlHelper.ActionLink(model.Metadata.Name, model);
        }
        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, IPageModel model) {
            return htmlHelper.ActionLink(linkText, "index", new { model });
        }
        /// <summary>
        /// Get the attribute of a specific type, returns null if not exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute {
            T attribute = null;

            var attributes = type.GetCustomAttributes(true);
            foreach (var attributeInType in attributes) {
                if (typeof(T).IsAssignableFrom(attributeInType.GetType()))
                    attribute = (T)attributeInType;
            }

            return attribute;
        }
    }
}
