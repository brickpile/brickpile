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
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace BrickPile.UI.Common {
    public static class HtmlExtensions {
        /// <summary>
        /// Sections the specified HTML helper.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="template">The template.</param>
        /// <param name="addToSection">The add to section.</param>
        /// <returns></returns>
        /// <example>
        /// Example of usage, both css and javascript will be added at the bottom of the page
        /// @Html.Section(@<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>, "scripts")
        /// @Html.Section(@<link rel="stylesheet" href="~/Styles/bootstrap.min.css" />, "styles")
        /// </example>
        public static MvcHtmlString Section(this HtmlHelper htmlHelper, Func<object, HelperResult> template, string addToSection) {
            htmlHelper.ViewContext.HttpContext.Items[String.Concat("_", addToSection, "_", Guid.NewGuid())] = new List<Func<object,HelperResult>> {template};
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Sections the specified HTML helper.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="template1">The template1.</param>
        /// <param name="template2">The template2.</param>
        /// <param name="addToSection">The add to section.</param>
        /// <returns></returns>
        public static MvcHtmlString Section(this HtmlHelper htmlHelper, Func<object, HelperResult> template1,Func<object, HelperResult> template2, string addToSection) {
            htmlHelper.ViewContext.HttpContext.Items[String.Concat("_", addToSection, "_", Guid.NewGuid())] =
                new List<Func<object, HelperResult>> { template1, template2 };
            return MvcHtmlString.Empty;
        }
        /// <summary>
        /// Sections the specified HTML helper.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="template1">The template1.</param>
        /// <param name="template2">The template2.</param>
        /// <param name="template3">The template3.</param>
        /// <param name="addToSection">The add to section.</param>
        /// <returns></returns>
        public static MvcHtmlString Section(this HtmlHelper htmlHelper, Func<object, HelperResult> template1, Func<object, HelperResult> template2, Func<object,HelperResult> template3, string addToSection) {
            htmlHelper.ViewContext.HttpContext.Items[String.Concat("_", addToSection, "_", Guid.NewGuid())] =
                new List<Func<object, HelperResult>> { template1, template2, template3 };
            return MvcHtmlString.Empty;
        }
        /// <summary>
        /// Renders the section.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static IHtmlString RenderSection(this HtmlHelper htmlHelper, string sectionName) {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys) {
                if (key.ToString().StartsWith(String.Concat("_", sectionName, "_"))) {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as List<Func<object, HelperResult>>;
                    if (template != null) {
                        foreach (var func in template) {
                            htmlHelper.ViewContext.Writer.Write(func(null));    
                        }
                    }
                }
            }
            return MvcHtmlString.Empty;
        }
        
    }
}