using System.Web.Mvc;

namespace Stormbreaker.Dashboard.Web.Mvc.Html {
    public static class ActionLinkExtensions {
        public static MvcHtmlString EditActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues) {
            return EditActionLink(htmlHelper, linkText, actionName, routeValues, string.Empty);
        }
        public static MvcHtmlString EditActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues,string @class) {

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var url = urlHelper.Action("delete", routeValues);

            var delTagBuilder = new TagBuilder("span");
            delTagBuilder.MergeAttribute("title","Delete page");
            delTagBuilder.AddCssClass("delete");

            delTagBuilder.MergeAttribute("data-val", url);

            url = urlHelper.Action("add", routeValues);

            var addTagBuilder = new TagBuilder("span");
            addTagBuilder.MergeAttribute("title","Add child page");
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
            url = urlHelper.Action(actionName, routeValues);
            tagBuilder.MergeAttribute("href", url);
            if(!string.IsNullOrEmpty(@class)) {
                tagBuilder.AddCssClass(@class);
            }

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}