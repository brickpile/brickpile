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
    public static class MenuHelper {
        /// <summary>
        /// Gets the current model.
        /// </summary>
        private static IPageModel CurrentModel {
            get { return ((MvcHandler) HttpContext.Current.Handler).RequestContext.RouteData.GetCurrentModel<IPageModel>(); }
        }
        /// <summary>
        /// Responsible for creating a navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <param name="showRootPage">if set to <c>true</c> [show root page].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, bool enableDisplayInMenu = true) {
            return Menu(html, pages, itemContent, itemContent,enableDisplayInMenu);
        }
        /// <summary>
        /// Responsible for creating a main navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="selectedItemContent">Content for selected links</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <param name="showRootPage">if set to <c>true</c> [show root page].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true) {
            return Menu(html, pages, itemContent, selectedItemContent, itemContent,enableDisplayInMenu);
        }
        /// <summary>
        /// Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <param name="showRootPage">if set to <c>true</c> [show root page].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, Func<IPageModel, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) {
            return Menu(html, pages, itemContent, selectedItemContent, expandedItemContent, null, enableDisplayInMenu);
        }
        /// <summary>
        /// Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element. The attributes are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <param name="showRootPage">if set to <c>true</c> [show root page].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, IEnumerable<IPageModel> pages, Func<IPageModel, MvcHtmlString> itemContent, Func<IPageModel, MvcHtmlString> selectedItemContent, Func<IPageModel, MvcHtmlString> expandedItemContent, object htmlAttributes, bool enableDisplayInMenu = true) {
            if (pages == null) {
                return MvcHtmlString.Empty;
            }

            // create unordered list
            var ul = new TagBuilder("ul");
            // merge html attributes
            ul.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var nodes = pages.AsHierarchy();

            // only render the top level items
            var items = nodes.Where(x => x.Depth == 1);

            IPageModel home;
            if(!items.Any()) {
                // don't render anything
                // render home if it's published
                home = pages.SingleOrDefault(model => model.Parent == null);
                if (home != null) {
                    if(enableDisplayInMenu && !home.Metadata.DisplayInMenu) {
                        return MvcHtmlString.Empty;
                    }
                    RenderLi(ul, home, home.Equals(CurrentModel) ? selectedItemContent : itemContent);
                    return MvcHtmlString.Create(ul.ToString());
                }
                return MvcHtmlString.Empty;
            }

            // add home item if it's visible
            home = pages.SingleOrDefault(model => model.Parent == null);
            if (home != null) {
                if(enableDisplayInMenu && home.Metadata.DisplayInMenu) {
                    RenderLi(ul, home, home.Equals(CurrentModel) ? selectedItemContent : itemContent);
                }
            }

            // filter pages that are not visible in menu
            if(enableDisplayInMenu) {
                items = items.Where(x => x.Entity.Metadata.DisplayInMenu);
            }

            foreach (var item in items) {
                RenderLi(ul, item.Entity, item.Entity.Equals(CurrentModel) ? selectedItemContent : (item.Expanded ? expandedItemContent : itemContent));
            }

            return MvcHtmlString.Create(ul.ToString());
        }
        /// <summary>
        /// Responsible for renderingen the li element with it's content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="item">The item.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        private static void RenderLi<T>(TagBuilder tagBuilder, T item, Func<T, MvcHtmlString> itemContent) {
            tagBuilder.InnerHtml += new TagBuilder("li") {InnerHtml = itemContent(item).ToHtmlString()}.ToString(TagRenderMode.Normal);
        }  
    }
}
