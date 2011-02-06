using System;
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
    public static class MenuHelper {
        /// <summary>
        /// Responsible for creating a navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="structureInfo">Hierarchical structure info</param>
        /// <param name="itemContent">Default content for links</param>
        /// <returns></returns>
        public static string Menu(this HtmlHelper html, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent)
        {
            return Menu(html, null, structureInfo, itemContent);
        }
        /// <summary>
        /// Responsible for creating a navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="id">Id of the unordered list</param>
        /// <param name="structureInfo">Hierarchical structure info</param>
        /// <param name="itemContent">Default content for links</param>
        /// <returns></returns>
        public static string Menu(this HtmlHelper html, string id, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent)
        {
            return Menu(html, id, null, structureInfo, itemContent);
        }
        /// <summary>
        /// Responsible for creating a navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="id">Id of the unordered list</param>
        /// <param name="currentModel">The current page in the current request</param>
        /// <param name="structureInfo">Hierarchical structure info</param>
        /// <param name="itemContent">Default content for links</param>
        /// <returns></returns>
        public static string Menu(this HtmlHelper html, string id, IPageModel currentModel, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent)
        {
            return Menu(html, id, currentModel, structureInfo, itemContent, itemContent);
        }
        /// <summary>
        /// Responsible for creating a main navigation based on an unordered list
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="id">The id of the unordered list</param>
        /// <param name="currentModel"></param>
        /// <param name="structureInfo"></param>
        /// <param name="itemContent">Default content for links</param>
        /// <param name="selectedItemContent">Content for selected links</param>
        /// <returns></returns>
        public static string Menu<T>(this HtmlHelper html, string id, T currentModel, IStructureInfo structureInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IPageModel
        {
            // only render the top level items
            var items = structureInfo.HierarchicalStructure.Where(x => x.Depth == 1).OrderBy(x => x.Entity.MetaData.Name);

            var sb = new StringBuilder();
            if(string.IsNullOrEmpty(id)) {
                sb.AppendLine("<ul>");
            }
            else {
                sb.AppendFormat("<ul id=\"{0}\">", id);
            }

            foreach (var item in items)
            {
                RenderLi(sb, "<li>{0}</li>", (T)item.Entity, item.Entity.Equals(currentModel) ? selectedItemContent : itemContent);
            }

            sb.AppendLine("</ul>");
            return sb.ToString();
        }
        /// <summary>
        /// Responsible for renderingen the li element with it's content
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        /// <param name="itemContent"></param>
        private static void RenderLi<T>(StringBuilder sb, string format, T item, Func<T, MvcHtmlString> itemContent) where T : IPageModel {
            sb.AppendFormat(format, itemContent(item));
        }
    }
}
