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

using System.Web.Mvc;

namespace BrickPile.UI.Web.Mvc.Html {
    public static class ActionLinkExtension {
        public static MvcHtmlString EditActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues) {
            return EditActionLink(htmlHelper, linkText, actionName, routeValues, string.Empty);
        }
        public static MvcHtmlString EditActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues,string @class) {

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var url = urlHelper.Action("delete", routeValues);

            var delTagBuilder = new TagBuilder("span");
            delTagBuilder.SetInnerText("Delete");
            delTagBuilder.MergeAttribute("title", "Delete page");
            delTagBuilder.AddCssClass("delete");

            delTagBuilder.MergeAttribute("data-val", url);

            url = urlHelper.Action("add", routeValues);

            var addTagBuilder = new TagBuilder("span");
            addTagBuilder.SetInnerText("Add");
            addTagBuilder.MergeAttribute("title", "Add child page");
            addTagBuilder.AddCssClass("add");

            addTagBuilder.MergeAttribute("data-val", url);

            //var cutTagBuilder = new TagBuilder("span");
            //cutTagBuilder.MergeAttribute("title", "Cut page");
            //cutTagBuilder.AddCssClass("cut");

            //var pasteTagBuilder = new TagBuilder("span");
            //pasteTagBuilder.MergeAttribute("title", "Paste page");
            //pasteTagBuilder.AddCssClass("paste");

            //url = urlHelper.Action("edit", routeValues);

            //pasteTagBuilder.MergeAttribute("data-val", url);

            var tagBuilder = new TagBuilder("a")
            {
                InnerHtml = linkText + delTagBuilder.ToString(TagRenderMode.Normal) + addTagBuilder.ToString(TagRenderMode.Normal)
            };
            

            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            urlHelper.Action(actionName, routeValues);
            tagBuilder.MergeAttribute("href", url);
            if(!string.IsNullOrEmpty(@class)) {
                tagBuilder.AddCssClass(@class);
            }

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}