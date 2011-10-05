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
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.Mvc.Html {
    public static class TreeViewHelper {
        //* *******************************************************************
        //*  Methods 
        //* *******************************************************************/
        public static string TreeView<T>(this HtmlHelper html, T currentItem, IStructureInfo structureInfo, Func<T, MvcHtmlString> itemContent, Func<T, MvcHtmlString> selectedItemContent) where T : IPageModel {

            if (structureInfo.Hierarchy == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("<ul>");
            RenderLi(sb, (T)structureInfo.RootModel, structureInfo.RootModel.Equals(currentItem) ? selectedItemContent : itemContent);

            var items = structureInfo.Hierarchy.Where(x => x.Depth == 1);

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