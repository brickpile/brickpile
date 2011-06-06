using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Stormbreaker.Dashboard.Web.Mvc.Html {
    public static class LabelExtensions {
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> ex, Func<object, HelperResult> template, string labelText = null) {
            var htmlFieldName = ExpressionHelper.GetExpressionText(ex);
            var metadata = htmlHelper.ViewData.ModelMetadata;
            string resolvedLabelText = labelText ?? metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(resolvedLabelText)) {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.InnerHtml = string.Format(
                "{0} {1}",
                resolvedLabelText,
                template(null).ToHtmlString()
            );
            return MvcHtmlString.Create(tag.ToString());

        }
    }
} 