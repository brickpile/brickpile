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
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.UI.Web.Routing;

namespace BrickPile.UI.Common {
    public static class Extensions {
        /// <summary>
        /// Creates the hierarchy.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="allItems">All items.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity>(IEnumerable<TEntity> allItems, TEntity parentItem, int depth) where TEntity : class, IPageModel {

            if (parentItem == null)
                parentItem = allItems.SingleOrDefault(i => i.Parent == null);

            if(parentItem == null) {
                yield break;
            }
            IEnumerable<TEntity> childs = allItems.Where(i => i.Parent != null && i.Parent.Id.Equals(parentItem.Id));

            if (childs.Any()) {
                depth++;

                foreach (var item in childs)
                    yield return
                        new HierarchyNode<TEntity>()
                            {
                                Entity = item,
                                ChildNodes = CreateHierarchy(allItems, item, depth),
                                Depth = depth,
                                Expanded = allItems.Any(x => x.Parent != null && x.Parent.Id.Equals(item.Id))
                            };
            }
        }
        /// <summary>
        /// Ases the hierarchy.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="allItems">All items.</param>
        /// <returns></returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(this IEnumerable<TEntity> allItems) where TEntity : class, IPageModel {
            return CreateHierarchy(allItems, default(TEntity), 0);
        }
        /// <summary>
        /// Registers the page route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="pathResolver">The path resolver.</param>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <returns></returns>
        //public static RouteCollection RegisterPageRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) {
        //    var pageRoute = new PageRoute(pathResolver, virtualPathResolver);
        //    routes.Add("PageRoute", pageRoute);
        //    return routes;
        //}

        /// <summary>
        /// Used for adding a page model to the RouteData object's DataTokens
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static RouteData ApplyCurrentModel(this RouteData data, string controllerName, string actionName, dynamic model) {
            data.Values[PageRoute.ControllerKey] = controllerName.Replace("Controller","");
            data.Values[PageRoute.ActionKey] = actionName;
            data.Values[PageRoute.ModelKey] = model;
            return data;
        }
        /// <summary>
        /// Applies the current structure info.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <returns></returns>
        public static RouteData ApplyCurrentStructureInfo(this RouteData data, IStructureInfo structureInfo) {
            data.Values[PageRoute.StructureInfoKey] = structureInfo;
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
        /// Actions the specified URL helper.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, IPageModel model) {
            return urlHelper.Action("index", new {model});
        }
        /// <summary>
        /// Actions the specified URL helper.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper,string actionName, IPageModel model) {
            return urlHelper.Action(actionName, new { model });
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
        /// <summary>
        /// Radioes the button for select list.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="listOfValues">The list of values.</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> listOfValues) {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();
            if (listOfValues != null) {
                foreach (SelectListItem item in listOfValues) {
                    var id = string.Format(
                        "{0}_{1}",
                        metaData.PropertyName,
                        item.Value
                    );

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id }).ToHtmlString();
                    sb.AppendFormat(
                        "<label for=\"{0}\">{2} {1}</label>",
                        id,
                        HttpUtility.HtmlEncode(item.Text),
                        radio
                    );
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        private const string DateFormat = "{0} {1} {2}";
        /// <summary>
        /// Formats the date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static string FormatDate(this DateTime? dateTime) {
            var difference = DateTime.Now.Subtract((DateTime) dateTime);

            if (difference.Days >= 1) {
                if (difference.Days == 1) {
                    return "yesterday";
                }
                return dateTime.Value.ToShortDateString();
            }
            if (difference.Hours == 0) {
                if (difference.Minutes == 0) {
                    return "just now";
                }
                return string.Format(DateFormat, difference.Minutes, difference.Minutes == 1 ? "minute" : "minutes", "ago");
            }
            return string.Format(DateFormat, difference.Hours, difference.Hours == 1 ? "hour" : "hours", "ago");
        }
        /// <summary>
        /// Gets the available page models.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static List<Type> GetAvailablePageModels(this HtmlHelper helper) {
            if(_availablePageModels == null) {
                _availablePageModels = new List<Type>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var type in assembly.GetTypes()) {
                        if (type.GetCustomAttributes(typeof(PageTypeAttribute), true).Length > 0) {
                            _availablePageModels.Add(type);
                        }
                    }
                }                
            }
            return _availablePageModels;
        }

        private static List<Type> _availablePageModels;
    }
}
