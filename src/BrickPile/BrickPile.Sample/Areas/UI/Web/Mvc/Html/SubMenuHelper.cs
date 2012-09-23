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
using System.Web.Routing;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;

namespace BrickPile.UI.Web.Mvc.Html {
    /// <summary>
    /// Extension methods for the <see cref="HtmlHelper"/> object.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class SubMenuHelper {
        /// <summary>
        /// Gets the current model.
        /// </summary>
        private static IPageModel CurrentPage {
            get { return ((MvcHandler) HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentPage<IPageModel>(); }
        }
        /// <summary>
        /// Gets the structure info.
        /// </summary>
        private static IStructureInfo StructureInfo {
            get {
                return ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.Values["StructureInfo"] as StructureInfo;
            }
        }
        /// <summary>
        /// Subs the menu.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPageModel, MvcHtmlString> itemContent, bool enableDisplayInMenu = true) {
            return SubMenu(html, StructureInfo.NavigationContext, itemContent, itemContent);
        }
        /// <summary>
        /// Subs the menu.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true) {
            return SubMenu(html, StructureInfo.NavigationContext, itemContent, selectedItemContent, itemContent);
        }
        /// <summary>
        /// Subs the menu.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, Func<IPageModel, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) {
            return SubMenu(html, StructureInfo.NavigationContext, itemContent, selectedItemContent, expandedItemContent, null);
        }
        /// <summary>
        /// Subs the menu.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, Func<IPageModel, MvcHtmlString> expandedItemContent, object htmlAttributes, bool enableDisplayInMenu = true) {
            return SubMenu(html, StructureInfo.NavigationContext, itemContent, selectedItemContent, expandedItemContent,htmlAttributes, enableDisplayInMenu);
        }
        /// <summary>
        /// Creates a hierarchical unordered navigation list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, bool enableDisplayInMenu = true) {
            return SubMenu(html, pages, itemContent, itemContent);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true) {
            return SubMenu(html, pages, itemContent, selectedItemContent, itemContent);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, Func<IPageModel, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) {
            return SubMenu(html, pages, itemContent, selectedItemContent, expandedItemContent, null);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, Func<IPageModel, MvcHtmlString> expandedItemContent, object htmlAttributes, bool enableDisplayInMenu = true) {
            if (pages == null) {
                return MvcHtmlString.Empty;
            }

            var hierarchyNodes = pages.AsHierarchy();

            var item = hierarchyNodes.SingleOrDefault(x => x.Expanded);

            if (item == null || !item.ChildNodes.Any())
                return MvcHtmlString.Empty;

            var ul = new TagBuilder("ul");
            // merge html attributes
            ul.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            
            foreach (var node in item.ChildNodes) {
                if(enableDisplayInMenu && node.Entity.Metadata.DisplayInMenu) {
                    RenderLi(ul, node.Entity, node.Entity.Equals(CurrentPage) ? selectedItemContent : (node.Expanded ? expandedItemContent : itemContent));
                }
                AppendChildrenRecursive(ul, node, x => x.ChildNodes, itemContent, selectedItemContent, expandedItemContent, enableDisplayInMenu);
            }

            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));            
        }

        /// <summary>
        /// Creates an unordered hierarchical list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="rootNode">The root node.</param>
        /// <param name="childrenProperty">The children property.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        private static void AppendChildrenRecursive<T>(TagBuilder tagBuilder, IHierarchyNode<IPageModel> rootNode, Func<IHierarchyNode<IPageModel>, IEnumerable<IHierarchyNode<IPageModel>>> childrenProperty, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent, Func<T, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) {
            var children = childrenProperty(rootNode);
            
            if(children != null && enableDisplayInMenu) {
                children = children.Where(x => x.Entity.Metadata.DisplayInMenu);
            }

            if (!children.Any()) {
                tagBuilder.InnerHtml += new TagBuilder("li").ToString(TagRenderMode.EndTag);
                return;
            }

            var ul = new TagBuilder("ul");

            foreach (var item in children) {
                RenderLi(ul, (T)item.Entity, item.Entity.Id.Equals(CurrentPage) ? selectedItemContent : (item.Expanded ? expandedItemContent : itemContent));
                AppendChildrenRecursive(ul, item, childrenProperty, itemContent, selectedItemContent, expandedItemContent);
            }

            tagBuilder.InnerHtml += ul.ToString(TagRenderMode.Normal);
            tagBuilder.InnerHtml += new TagBuilder("li").ToString(TagRenderMode.EndTag);
        }

        /// <summary>
        /// Responsible for renderingen the li element with it's content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="item">The item.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        private static void RenderLi<T>(TagBuilder tagBuilder, T item, Func<T, MvcHtmlString> itemContent) {
            tagBuilder.InnerHtml += new TagBuilder("li").ToString(TagRenderMode.StartTag) + itemContent(item);

        }  
    }
}