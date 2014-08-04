using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BrickPile.Core.Routing;
using BrickPile.Domain;
using IPage = BrickPile.Core.IPage;

namespace BrickPile.UI.Common {
    public static class Extensions {

        /// <summary>
        /// Applies the current page.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static RouteData ApplyCurrentPage(this RouteData data, dynamic model) {
            data.Values[PageRoute.CurrentPageKey] = model;
            return data;
        }

        /// <summary>
        /// Used for adding a page model to the RouteData object's DataTokens
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static RouteData ApplyCurrentPage(this RouteData data, string controllerName, string actionName, dynamic model) {
            data.Values[PageRoute.ControllerKey] = controllerName.Replace("Controller", "");
            data.Values[PageRoute.ActionKey] = actionName;
            data.Values[PageRoute.CurrentPageKey] = model;
            return data;
        }

        /// <summary>
        /// Applies the current structure info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T GetCurrentPage<T>(this RouteData data) {
            return (T)data.Values[PageRoute.CurrentPageKey];
        }

        /// <summary>
        /// Adds the query param.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string AddQueryParam(this string source, string key, string value) {
            string delim;
            if ((source == null) || !source.Contains("?")) {
                delim = "?";
            }
            else if (source.EndsWith("?") || source.EndsWith("&")) {
                delim = string.Empty;
            }
            else {
                delim = "&";
            }
            return source + delim + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IPage model) {
            return htmlHelper.ActionLink(model.Metadata.Name, model);
        }

        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IPage model, object htmlAttributes) {
            return htmlHelper.ActionLink(model.Metadata.Name, model, htmlAttributes);
        }

        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, IPage model) {
            return htmlHelper.ActionLink(linkText, model, null);
        }

        /// <summary>
        /// Actions the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="model">The model.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, IPage model,
                                               object htmlAttributes) {
            return htmlHelper.ActionLink(linkText, "index", new {currentPage = model}, htmlAttributes);
        }

        /// <summary>
        /// Actions the specified URL helper.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, IPage model) {
            return urlHelper.Action("index", new { currentPage = model });
        }
        /// <summary>
        /// Actions the specified URL helper.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, IPage model) {
            return urlHelper.Action(actionName, new { currentPage = model });
        }

        /// <summary>
        /// Get the attribute of a specific type, returns null if not exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute {
            T attribute = null;

            var attributes = type.GetCustomAttributes(true);
            foreach (var attributeInType in attributes) {
                if (typeof (T).IsAssignableFrom(attributeInType.GetType()))
                    attribute = (T) attributeInType;
            }

            return attribute;
        }

        /// <summary>
        /// Radioes the button for select list.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="listOfValues">The list of values.</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                                Expression<Func<TModel, TProperty>>
                                                                                    expression,
                                                                                IEnumerable<SelectListItem> listOfValues) {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();
            if (listOfValues != null) {
                foreach (SelectListItem item in listOfValues) {
                    var id = string.Format(
                        "{0}_{1}",
                        metaData.PropertyName,
                        item.Value
                        );

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new {id}).ToHtmlString();
                    sb.AppendFormat(
                        "<label for=\"{0}\">{2} {1}</label>",
                        id,
                        HttpUtility.HtmlEncode(item.Text),
                        radio
                        );
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        private const string DateFormat = "{0} {1} {2}";

        /// <summary>
        /// Formats the date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static string FormatDate(this DateTime? dateTime) {
            var difference = DateTime.Now.Subtract((DateTime) dateTime);

            if (difference.Days >= 1) {
                if (difference.Days == 1) {
                    return "yesterday";
                }
                return dateTime.Value.ToShortDateString();
            }
            if (difference.Hours == 0) {
                if (difference.Minutes == 0) {
                    return "just now";
                }
                return string.Format(DateFormat, difference.Minutes, difference.Minutes == 1 ? "minute" : "minutes",
                                     "ago");
            }
            return string.Format(DateFormat, difference.Hours, difference.Hours == 1 ? "hour" : "hours", "ago");
        }

        /// <summary>
        /// Gets the available page models.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static List<Type> GetAvailablePageModels(this HtmlHelper helper) {
            if (_availablePageModels == null) {
                _availablePageModels = new List<Type>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var type in assembly.GetTypes()) {
                        if (type.GetCustomAttributes(typeof(ContentTypeAttribute), true).Length > 0) {
                            _availablePageModels.Add(type);
                        }
                    }
                }
            }
            return _availablePageModels;
        }

        private static List<Type> _availablePageModels;
    }
}
