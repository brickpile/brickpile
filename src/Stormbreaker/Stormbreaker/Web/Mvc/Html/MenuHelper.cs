using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Stormbreaker.Models;
using Stormbreaker.Web.UI.Navigation;

namespace Stormbreaker.Web.Mvc.Html {
    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class MenuHelper {

        //* *******************************************************************
        //*  Methods 
        //* *******************************************************************/
        #region public static string Menu<T>(this HtmlHelper html, INavigationInfo navigationInfo, Func<T, MvcHtmlString> itemContent)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="navigationInfo"></param>
        /// <param name="itemContent"></param>
        /// <returns></returns>
        public static string Menu<T>(this HtmlHelper html, INavigationInfo navigationInfo, Func<T, MvcHtmlString> itemContent) where T : IContentItem
        {
            return Menu(html, navigationInfo, itemContent);
        }
        #endregion
        #region public static string Menu<T>(this HtmlHelper html, INavigationInfo navigationInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="navigationInfo"></param>
        /// <param name="itemContent"></param>
        /// <param name="selectedItemContent"></param>
        /// <returns></returns>
        public static string Menu<T>(this HtmlHelper html, INavigationInfo navigationInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IContentItem
        {
            if (navigationInfo.NavigationItems == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            sb.AppendLine("<ul>");

            RenderLi(sb, "<li>{0}</li>", (T)navigationInfo.StartItem, navigationInfo.StartItem.Id.Equals(navigationInfo.CurrentItem.Id) ? selectedItemContent : itemContent);

            AppendChildren(sb, (T)navigationInfo.CurrentItem, navigationInfo.NavigationItems, itemContent, selectedItemContent);

            sb.AppendLine("</ul>");

            return sb.ToString();
        }
        #endregion

        #region private static void AppendChildren<T>(StringBuilder sb, T currentPage, IEnumerable<INavigationItem> children, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="currentPage"></param>
        /// <param name="children"></param>
        /// <param name="itemContent"></param>
        /// <param name="selectedItemContent"></param>
        private static void AppendChildren<T>(StringBuilder sb, T currentPage, IEnumerable<INavigationItem> children, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IContentItem
        {
            foreach (var child in children)
            {
                RenderLi(sb, "<li>{0}</li>", (T)child.ContentItem, child.ContentItem.Id.Equals(currentPage.Id) ? selectedItemContent : itemContent);
            }
        }
        #endregion
        #region private static void RenderLi<T>(StringBuilder sb, string format, T item, Func<T, MvcHtmlString> itemContent)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="format"></param>
        /// <param name="item"></param>
        /// <param name="itemContent"></param>
        private static void RenderLi<T>(StringBuilder sb, string format, T item, Func<T, MvcHtmlString> itemContent) where T : IContentItem
        {
            sb.AppendFormat(format, itemContent(item));
        }
        #endregion
    }
}
