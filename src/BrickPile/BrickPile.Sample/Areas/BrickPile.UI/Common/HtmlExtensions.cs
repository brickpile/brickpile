using System;
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
            htmlHelper.ViewContext.HttpContext.Items[String.Concat("_", addToSection, "_", Guid.NewGuid())] = template;
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
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null) {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }
        
    }
}