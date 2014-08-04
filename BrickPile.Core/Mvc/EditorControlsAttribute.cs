using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Extensions;

namespace BrickPile.Core.Mvc
{    
    public class EditorControlsAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="EditorControlsAttribute" /> is disabled.
        ///     When set to <c>true</c> on a <see cref="Controller" /> the editor controls will not be available.
        /// </summary>
        /// <value>
        ///     <c>true</c> if disable; otherwise, <c>false</c>.
        /// </value>
        public bool Disable { get; set; }

        public EditorControlsAttribute() {
            this.Disable = false;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext) {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated && !this.Disable)
            {
                using (var sw = new StringWriter())
                {
                    filterContext.Controller.ViewData.Model = filterContext.RouteData.GetCurrentPage<IPage>();
                    ViewEngineResult viewResult =
                        ViewEngines.Engines.FindPartialView(filterContext.Controller.ControllerContext,
                            "~/Areas/UI/Views/Shared/_UIControls.cshtml");
                    var viewContext = new ViewContext(filterContext.Controller.ControllerContext, viewResult.View,
                        filterContext.Controller.ViewData, filterContext.Controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(filterContext.Controller.ControllerContext, viewResult.View);
                    HttpResponseBase response = filterContext.HttpContext.Response;
                    response.Filter = new AddUiToolsFilter(response.Filter, sw.GetStringBuilder().ToString());
                }
            }
            else
            {
                base.OnResultExecuted(filterContext);
            }
        }

        private class AddUiToolsFilter : MemoryStream
        {
            private readonly string htmlToAppend;
            private readonly Stream response;

            public AddUiToolsFilter(Stream response, string htmlToAppend) {
                this.response = response;
                this.htmlToAppend = htmlToAppend;
            }

            public override void Write(byte[] buffer, int offset, int count) {
                string html = Encoding.UTF8.GetString(buffer);
                html = html.Replace("</body>", this.htmlToAppend + "</body>");                
                buffer = Encoding.UTF8.GetBytes(html);
                this.response.Write(buffer, offset, buffer.Length);
            }
        }
    }
}