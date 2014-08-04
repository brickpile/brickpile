using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;

namespace BrickPile.Core.Mvc.Html
{
    /// <summary>
    ///     Extension methods for the <see cref="HtmlHelper" /> object.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class MenuHelper
    {
        /// <summary>
        ///     Gets the navigation context.
        /// </summary>
        /// <value>
        ///     The navigation context.
        /// </value>
        private static INavigationContext NavigationContext {
            get { return ((MvcHandler) HttpContext.Current.Handler).RequestContext.RouteData.GetNavigationContext(); }
        }

        /// <summary>
        ///     Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, Func<IPage, MvcHtmlString> itemContent,
            bool enableDisplayInMenu = true) {
            return Menu(html, NavigationContext, itemContent, enableDisplayInMenu);
        }

        /// <summary>
        ///     Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, INavigationContext navigationContext,
            Func<IPage, MvcHtmlString> itemContent, bool enableDisplayInMenu = true) {
            return Menu(html, navigationContext, itemContent, itemContent, enableDisplayInMenu);
        }

        /// <summary>
        ///     Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, Func<IPage, MvcHtmlString> itemContent,
            Func<IPage, MvcHtmlString> selectedItemContent, bool enableDisplayInMenu = true) {
            return Menu(html, NavigationContext, itemContent, selectedItemContent, itemContent, enableDisplayInMenu);
        }

        /// <summary>
        ///     Responsible for creating a main navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="selectedItemContent">Content for selected links</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, INavigationContext navigationContext,
            Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent,
            bool enableDisplayInMenu = true) {
            return Menu(html, navigationContext, itemContent, selectedItemContent, itemContent, enableDisplayInMenu);
        }

        /// <summary>
        ///     Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, INavigationContext navigationContext,
            Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent,
            Func<IPage, MvcHtmlString> expandedItemContent, bool enableDisplayInMenu = true) {
            return Menu(html, navigationContext, itemContent, selectedItemContent, expandedItemContent, null,
                enableDisplayInMenu);
        }

        /// <summary>
        ///     Menus the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="itemContent">Content of the item.</param>
        /// <param name="selectedItemContent">Content of the selected item.</param>
        /// <param name="expandedItemContent">Content of the expanded item.</param>
        /// <param name="htmlAttributes">
        ///     An object that contains the HTML attributes for the element. The attributes are retrieved
        ///     through reflection by examining the properties of the object. The object is typically created by using object
        ///     initializer syntax.
        /// </param>
        /// <param name="enableDisplayInMenu">if set to <c>true</c> [enable display in menu].</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper html, INavigationContext navigationContext,
            Func<IPage, MvcHtmlString> itemContent, Func<IPage, MvcHtmlString> selectedItemContent,
            Func<IPage, MvcHtmlString> expandedItemContent, object htmlAttributes, bool enableDisplayInMenu = true) {
            if (navigationContext == null)
            {
                return MvcHtmlString.Empty;
            }

            // create unordered list
            var ul = new TagBuilder("ul");
            // merge html attributes
            ul.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            IEnumerable<dynamic> nodes = navigationContext.CurrentContext.FilterForDisplay().AsHierarchy();

            // only render the top level items
            IEnumerable<dynamic> items = nodes.Where(x => x.Depth == 1);

            IPage home;
            if (!items.Any())
            {
                // don't render anything
                // render home if it's published
                home = navigationContext.CurrentContext.SingleOrDefault(model => model.Parent == null);
                if (home != null)
                {
                    if (enableDisplayInMenu && !home.Metadata.DisplayInMenu)
                    {
                        return MvcHtmlString.Empty;
                    }
                    RenderLi(ul, home,
                        home.Id.Replace("/draft", "").Equals(NavigationContext.CurrentPage.Id.Replace("/draft", ""))
                            ? selectedItemContent
                            : itemContent);
                    return MvcHtmlString.Create(ul.ToString());
                }
                return MvcHtmlString.Empty;
            }

            // add home item if it's visible
            home = navigationContext.CurrentContext.SingleOrDefault(model => model.Parent == null);
            if (home != null)
            {
                if (enableDisplayInMenu && home.Metadata.DisplayInMenu)
                {
                    RenderLi(ul, home,
                        home.Id.Replace("/draft", "").Equals(navigationContext.CurrentPage.Id.Replace("/draft", ""))
                            ? selectedItemContent
                            : itemContent);
                }
            }

            // filter pages that are not visible in menu
            if (enableDisplayInMenu)
            {
                items = items.Where(x => x.Entity.Metadata.DisplayInMenu);
            }

            foreach (dynamic item in items)
            {
                RenderLi(ul, item.Entity,
                    ((IPage) item.Entity).Id.Replace("/draft", "").Equals(navigationContext.CurrentPage.Id.Replace("/draft", ""))
                        ? selectedItemContent
                        : (item.Expanded ? expandedItemContent : itemContent));
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        /// <summary>
        ///     Responsible for renderingen the li element with it's content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="item">The item.</param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        private static void RenderLi<T>(TagBuilder tagBuilder, T item, Func<T, MvcHtmlString> itemContent) {
            tagBuilder.InnerHtml +=
                new TagBuilder("li") {InnerHtml = itemContent(item).ToHtmlString()}.ToString(TagRenderMode.Normal);
        }
    }
}