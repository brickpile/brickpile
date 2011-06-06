using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Dashboard.Web.Mvc.Html {
    public static class TreeViewHelper {
        //* *******************************************************************
        //*  Methods 
        //* *******************************************************************/
        public static string TreeView<T>(this HtmlHelper html, T currentItem, IStructureInfo structureInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IPageModel {

            if (structureInfo.HierarchicalStructure == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("<ul>");
            RenderLi(sb, (T)structureInfo.RootModel, structureInfo.RootModel.Equals(currentItem) ? selectedItemContent : itemContent);

            var items = structureInfo.HierarchicalStructure.Where(x => x.Depth == 1);

            if(items.Count() < 1) {
                sb.AppendLine("</ul></li>");
                return sb.ToString();
            }

            sb.AppendLine("<ul>");
            foreach (var item in items)
            {
                RenderLi(sb, (T) item.Entity, item.Entity.Equals(currentItem) ? selectedItemContent : itemContent);
                AppendChildrenRecursive(sb, currentItem, item, x => x.ChildNodes, itemContent, selectedItemContent);
            }
            sb.AppendLine("</ul></li>");
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static void AppendChildrenRecursive<T>(StringBuilder sb, IPageModel currentItem, IHierarchyNode<IPageModel> currentNode, Func<IHierarchyNode<IPageModel>, IEnumerable<IHierarchyNode<IPageModel>>> childrenProperty, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent)
        {

            var children = childrenProperty(currentNode);

            if (children == null || children.Count() == 0)
            {
                sb.AppendLine("</li>");
                return;
            }

            sb.AppendLine("<ul>");

            foreach (var item in children.OrderBy(x => x.Entity.Id))
            {
                RenderLi(sb, (T)item.Entity, item.Entity.Id.Equals(currentItem.Id) ? selectedItemContent : itemContent);
                AppendChildrenRecursive(sb, currentItem, item, childrenProperty, itemContent, selectedItemContent);
            }

            sb.AppendLine("</ul></li>");
        }

        private static void RenderLi<T>(StringBuilder sb, T item, Func<T, MvcHtmlString> itemContent) {
            sb.AppendFormat("<li data-item-id=\"{0}\">{1}", ((IPageModel)item).Id, itemContent(item));
        }        
    }
}