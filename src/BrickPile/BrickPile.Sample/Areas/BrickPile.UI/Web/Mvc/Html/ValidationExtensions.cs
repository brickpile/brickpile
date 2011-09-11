using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BrickPile.UI.Web.Mvc.Html {
    public static class ValidationExtensions {
        /// <summary>
        /// Validations the message label for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="errorClass">The error class.</param>
        /// <returns></returns>
         public static MvcHtmlString ValidationMessageLabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string errorClass = "error") {
             var elementName = ExpressionHelper.GetExpressionText(expression);
             var normal = html.ValidationMessageFor(expression);
             if (normal != null) {
                 var newValidator = Regex.Replace(normal.ToHtmlString(), @"<span([^>]*)>([^<]*)</span>", string.Format("<label for=\"{0}\" $1><span>$2</span></label>", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(elementName))), RegexOptions.IgnoreCase);
                 return MvcHtmlString.Create(newValidator);
             }
             return null;
         }
    }
}