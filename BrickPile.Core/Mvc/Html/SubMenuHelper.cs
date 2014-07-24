using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing;

namespace BrickPile.Core.Mvc.Html
{
    /// <summary>
    /// Extension methods for the <see cref="HtmlHelper"/> object.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class SubMenuHelper
    {
        private static IPage CurrentPage
        {
            get { return ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.Values[PageRoute.CurrentPageKey] as IPage; }
        }

        /// <summary>
        /// Gets the navigation context.
        /// </summary>
        /// <value>
        /// The navigation context.
        /// </value>
        private static NavigationContext NavigationContext
        {
            get { return ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.GetNavigationContext(); }
        }

        /// <summary>
        /// Subs the menu.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPage, MvcHtmlString> itemContent, bool enableDisplayInMenu = true)
        {
            return SubMenu(html, NavigationContext, itemContent, itemContent);
        }

        /// <summary>
        /// Creates a hierarchical unordered navigation list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, NavigationContext navigationContext, Func<IPage, MvcHtmlString> itemContent, bool enableDisplayInMenu = true)
        {
            return SubMenu(html, navigationContext, itemContent, itemContent);
        }

        /// <summary>
        /// Subs the menu.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true)
        {
            return SubMenu(html, NavigationContext, itemContent, selectedItemContent, itemContent);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, NavigationContext navigationContext, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true)
        {
            return SubMenu(html, navigationContext, itemContent, selectedItemContent, itemContent);
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
        public static MvcHtmlString SubMenu(this HtmlHelper html, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, Func<IPage, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true)
        {
            return SubMenu(html, NavigationContext, itemContent, selectedItemContent, expandedItemContent, null);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, NavigationContext navigationContext, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, Func<IPage, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true)
        {
            return SubMenu(html, navigationContext, itemContent, selectedItemContent, expandedItemContent, null);
        }
        /// <summary>
        /// Responsible for creating a navigation tree based on a hierarchical structure
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="selectedItemContent">A lambda expression defining the content in each selected tree node</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString SubMenu(this HtmlHelper html, NavigationContext navigationContext, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, Func<IPage, MvcHtmlString> expandedItemContent, object htmlAttributes, bool enableDisplayInMenu = true)
        {
            if (navigationContext == null)
            {
                return MvcHtmlString.Empty;
            }

            var hierarchyNodes = navigationContext.CurrentContext.FilterForDisplay().AsHierarchy();

            var item = hierarchyNodes.SingleOrDefault(x => x.Expanded);

            if (item == null)
                return MvcHtmlString.Empty;

            var ul = new TagBuilder("ul");
            // merge html attributes
            ul.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            foreach (var node in item.ChildNodes)
            {
                if (enableDisplayInMenu && node.Entity.Metadata.DisplayInMenu)
                {
                    RenderLi(ul, node.Entity, node.Entity.Id.Replace("/draft", "").Equals(CurrentPage.Id.Replace("/draft", "")) ? selectedItemContent : (node.Expanded ? expandedItemContent : itemContent));
                }
                AppendChildrenRecursive(ul, node.ChildNodes, itemContent, selectedItemContent, expandedItemContent, enableDisplayInMenu);
            }

            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates an unordered hierarchical list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="children">The children.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        private static void AppendChildrenRecursive<T>(TagBuilder tagBuilder, IEnumerable<dynamic> children, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent, Func<T, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) where T : IPage
        {
            var items = children as dynamic[] ?? children.Where(x => x.Entity.Metadata.DisplayInMenu).ToArray();
            if (!items.Any())
            {
                tagBuilder.InnerHtml += new TagBuilder("li").ToString(TagRenderMode.EndTag);
                return;
            }

            var ul = new TagBuilder("ul");

            foreach (var item in items)
            {
                RenderLi(ul, (T)item.Entity, item.Entity.Id.Replace("/draft","").Equals(CurrentPage.Id.Replace("/draft","")) ? selectedItemContent : (item.Expanded ? expandedItemContent : itemContent));
                AppendChildrenRecursive(ul, item.ChildNodes, itemContent, selectedItemContent, expandedItemContent);
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
        private static void RenderLi<T>(TagBuilder tagBuilder, T item, Func<T, MvcHtmlString> itemContent) where T : IPage
        {
            tagBuilder.InnerHtml += new TagBuilder("li").ToString(TagRenderMode.StartTag) + itemContent(item);

        }
    }
}