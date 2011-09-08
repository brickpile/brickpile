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
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.Mvc.Html {
    public static class PageListExtension {
        public static string PageList(this HtmlHelper helper, IStructureInfo structureInfo, Func<IPageModel, MvcHtmlString> itemContent) {

            var node = structureInfo.HierarchicalStructure.SingleOrDefault(x => x.Entity.Id == structureInfo.CurrentModel.Id);

            if(node == null || node.ChildNodes == null || node.ChildNodes.Count() == 0) {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            foreach (var child in node.ChildNodes) {
                RenderLi(sb, "<li>{0}</li>", child.Entity, itemContent);    
            }
            sb.AppendLine("</table>");
            return sb.ToString();           
        }
        private static void RenderLi(StringBuilder sb, string format, IPageModel item, Func<IPageModel, MvcHtmlString> itemContent) {
            sb.AppendFormat(format, itemContent(item));
        }
    }
}