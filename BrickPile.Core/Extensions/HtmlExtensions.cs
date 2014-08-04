using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BrickPile.Core.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString UiControls(this HtmlHelper htmlHelper, IPage currentPage)
        {            
            return htmlHelper.Partial("~/Areas/UI/Views/Shared/UIControls.cshtml", currentPage);
        }
    }
}
