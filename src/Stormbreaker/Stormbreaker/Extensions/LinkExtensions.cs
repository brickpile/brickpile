using System.Web.Mvc;
using System.Web.Mvc.Html;
using Stormbreaker.Models;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// 
    /// </summary>
    public static class LinkExtensions {
        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IPageModel model) {
            return htmlHelper.ActionLink(model.MetaData.Name, model);
        }
        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, IPageModel model) {
            return htmlHelper.ActionLink(linkText,"index", new { model });
        }
    }
}