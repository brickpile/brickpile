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
    public static class MenuHelper
    {
        /// <summary>
        /// Gets the current model.
        /// </summary>
        private static IPage CurrentPage
        {
            get { return ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.Values[PageRoute.CurrentPageKey] as IPage; }
        }

        /// <summary>
        /// Menus the specified HTML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu<T>(this HtmlHelper html, IEnumerable<T> pages, Func<IPage, MvcHtmlString> itemContent, bool enableDisplayInMenu = true) where T : class, IPage
        {
            return Menu(html, pages, itemContent, itemContent, enableDisplayInMenu);
        }
        /// <summary>
        /// Responsible for creating a main navigation based on an unordered list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">HtmlHelper</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="selectedItemContent">Content for selected links</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu<T>(this HtmlHelper html, IEnumerable<T> pages, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true) where T : class, IPage
        {
            return Menu(html, pages, itemContent, selectedItemContent, itemContent, enableDisplayInMenu);
        }
        /// <summary>
        /// Menus the specified HTML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu<T>(this HtmlHelper html, IEnumerable<T> pages, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, Func<IPage, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) where T : class, IPage
        {
            return Menu(html, pages, itemContent, selectedItemContent, expandedItemContent, null, enableDisplayInMenu);
        }
        /// <summary>
        /// Menus the specified HTML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="pages">The pages.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element. The attributes are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu<T>(this HtmlHelper html, IEnumerable<T> pages, Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent, Func<IPage, MvcHtmlString> expandedItemContent, object htmlAttributes, bool enableDisplayInMenu = true) where T : class, IPage
        {
            if (pages == null)
            {
                return MvcHtmlString.Empty;
            }

            // create unordered list
            var ul = new TagBuilder("ul");
            // merge html attributes
            ul.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var nodes = pages.AsHierarchy();

            // only render the top level items
            var items = nodes.Where(x => x.Depth == 1);

            IPage home;
            if (!items.Any())
            {
                // don't render anything
                // render home if it's published
                home = pages.SingleOrDefault(model => model.Parent == null);
                if (home != null)
                {
                    if (enableDisplayInMenu && !home.Metadata.DisplayInMenu)
                    {
                        return MvcHtmlString.Empty;
                    }
                    RenderLi(ul, home, home.Equals(CurrentPage) ? selectedItemContent : itemContent);
                    return MvcHtmlString.Create(ul.ToString());
                }
                return MvcHtmlString.Empty;
            }

            // add home item if it's visible
            home = pages.SingleOrDefault(model => model.Parent == null);
            if (home != null)
            {
                if (enableDisplayInMenu && home.Metadata.DisplayInMenu)
                {
                    RenderLi(ul, home, home.Id.Replace("/draft", "").Equals(CurrentPage.Id.Replace("/draft", "")) ? selectedItemContent : itemContent);
                }
            }

            // filter pages that are not visible in menu
            if (enableDisplayInMenu)
            {
                items = items.Where(x => x.Entity.Metadata.DisplayInMenu);
            }

            foreach (var item in items)
            {
                RenderLi(ul, item.Entity, ((IPage)item.Entity).Id.Replace("/draft", "").Equals(CurrentPage.Id.Replace("/draft", "")) ? selectedItemContent : (item.Expanded ? expandedItemContent : itemContent));
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
        private static void RenderLi<T>(TagBuilder tagBuilder, T item, Func<T, MvcHtmlString> itemContent)
        {
            tagBuilder.InnerHtml += new TagBuilder("li") { InnerHtml = itemContent(item).ToHtmlString() }.ToString(TagRenderMode.Normal);
        }
    }
}