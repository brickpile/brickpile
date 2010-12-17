using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.Html {
    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class MenuHelper {

        //* *******************************************************************
        //*  Methods 
        //* *******************************************************************/
        public static string Menu<T>(this HtmlHelper html, T currentItem, IEnumerable<HierarchyNode<T>> structureInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IDocument
        {
            // only render the top level items
            var items = structureInfo.Where(x => x.Depth == 1);

            var sb = new StringBuilder();
            sb.AppendLine("<ul>");

            foreach (var item in items)
            {
                RenderLi(sb, "<li>{0}</li>", item.Entity, item.Entity.Equals(currentItem) ? selectedItemContent : itemContent);
            }

            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        public static string SubMenu<T>(this HtmlHelper html, IEnumerable<HierarchyNode<T>> structureInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemConten) where T : IDocument
        {
            var sb = new StringBuilder();
            AppendChildrenRecursive(sb, structureInfo.Last(), structureInfo, x => x.ChildNodes, itemContent, selectedItemConten);
            return sb.ToString();
        }
        private static void AppendChildrenRecursive<T>(StringBuilder sb, HierarchyNode<T> currentNode, IEnumerable<HierarchyNode<T>> structureInfo, Func<HierarchyNode<T>, IEnumerable<HierarchyNode<T>>> childrenProperty, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IDocument
        {

            var children = childrenProperty(currentNode);

            if (children == null || children.Count() == 0)
            {
                sb.AppendLine("</li>");
                return;
            }

            sb.AppendLine("<ul>");

            foreach (var item in children)
            {
                RenderLi(sb, "<li>{0}", item.Entity, itemContent);
                AppendChildrenRecursive(sb, item, structureInfo, childrenProperty, itemContent, selectedItemContent);
            }

            sb.AppendLine("</ul></li>");
        }
        private static void RenderLi<T>(StringBuilder sb, string format, T item, Func<T, MvcHtmlString> itemContent) where T : IDocument {
            sb.AppendFormat(format, itemContent(item));
        }
    }
}
