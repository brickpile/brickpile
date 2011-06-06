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
using System.Text;
using System.Web.Mvc;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.Html {
    /// <summary>
    /// Extension methods for the <see cref="HtmlHelper"/> object.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class SubMenuHelper {
        /// <summary>
        /// Creates a hierarchical unordered navigation list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="structureInfo">The structural navigation info</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <returns></returns>
        public static string SubMenu(this HtmlHelper html, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent) {
            return SubMenu(html, null, structureInfo, itemContent, itemContent);
        }
        /// <summary>
        /// Creates a hierarchical unordered navigation list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="id">The id of the unordered list</param>
        /// <param name="structureInfo">The structural navigation info</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <returns></returns>
        public static string SubMenu(this HtmlHelper html, string id, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent) {
            return SubMenu(html, id,structureInfo, itemContent, itemContent);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="id">The id of the unordered list</param>
        /// <param name="structureInfo">The structural navigation info</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <returns></returns>
        public static string SubMenu(this HtmlHelper html, string id, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent) {
            var item = structureInfo.HierarchicalStructure.Where(x => x.Expanded).SingleOrDefault();

            if (item == null || item.ChildNodes.Count() == 0)
                return string.Empty;

            var sb = new StringBuilder();

            if(string.IsNullOrEmpty(id)) {
                sb.AppendLine("<ul>");
            }
            else {
                sb.AppendFormat("<ul id=\"{0}\">", id);
            }

            foreach (var childNode in item.ChildNodes) {
                RenderLi(sb, childNode.Entity, childNode.Entity.Id.Equals(structureInfo.CurrentModel.Id) ? selectedItemContent : itemContent);
                AppendChildrenRecursive(sb, childNode, structureInfo.CurrentModel, x => x.ChildNodes, itemContent, selectedItemContent);    
            }

            sb.AppendLine("</ul>");
            return sb.ToString();
        }
        /// <summary>
        /// Creates an unordered hierarchical list
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="rootNode"></param>
        /// <param name="currentModel"></param>
        /// <param name="childrenProperty"></param>
        /// <param name="itemContent"></param>
        /// <param name="selectedItemContent"></param>
        private static void AppendChildrenRecursive<T>(StringBuilder sb, IHierarchyNode<IPageModel> rootNode, IPageModel currentModel, Func<IHierarchyNode<IPageModel>, IEnumerable<IHierarchyNode<IPageModel>>> childrenProperty, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) {
            var children = childrenProperty(rootNode);

            if (children.Count() == 0)
            {
                sb.AppendLine("</li>");
                return;
            }

            sb.AppendLine("<ul>");

            foreach (var item in children.OrderBy(x => x.Entity.Id))
            {
                RenderLi(sb, (T)item.Entity, item.Entity.Id.Equals(currentModel.Id) ? selectedItemContent : itemContent);
                AppendChildrenRecursive(sb, item, currentModel, childrenProperty, itemContent, selectedItemContent);
            }

            sb.AppendLine("</ul></li>");
        }
        /// <summary>
        /// Responsible for renderingen the li element with it's content
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        private static void RenderLi<T>(StringBuilder sb, T item, Func<T, MvcHtmlString> itemContent) {
            sb.AppendFormat("<li>{0}", itemContent(item));
        }   
    }
}