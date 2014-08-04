using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Extensions;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Renders the editor controls on pages when authorized.
    /// </summary>
    public class EditorControlsAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this
        ///     <see cref="EditorControlsAttribute" /> is disabled.
        ///     When set to
        ///     <c>true</c> on a
        ///     <see cref="Controller" /> the editor controls will not be available.
        /// </summary>
        /// <value>
        ///     <c>true</c> if disable; otherwise, <c>false</c>.
        /// </value>
        public bool Disable { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditorControlsAttribute" /> class.
        /// </summary>
        public EditorControlsAttribute()
        {
            this.Disable = false;
        }

        /// <summary>
        ///     Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated && !this.Disable)
            {
                using (var sw = new StringWriter())
                {
                    filterContext.Controller.ViewData.Model = filterContext.RouteData.GetCurrentPage<IPage>();
                    var viewResult =
                        ViewEngines.Engines.FindPartialView(filterContext.Controller.ControllerContext,
                            "~/Areas/UI/Views/Shared/_UIControls.cshtml");
                    var viewContext = new ViewContext(filterContext.Controller.ControllerContext, viewResult.View,
                        filterContext.Controller.ViewData, filterContext.Controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(filterContext.Controller.ControllerContext, viewResult.View);
                    var response = filterContext.HttpContext.Response;
                    response.Filter = new AddUiToolsFilter(response.Filter, sw.GetStringBuilder().ToString());
                }
            }
            else
            {
                base.OnResultExecuted(filterContext);
            }
        }

        /// <summary>
        ///     Represents the filter that alters the <see cref="HttpContext.Response" />.
        /// </summary>
        private class AddUiToolsFilter : MemoryStream
        {
            private readonly string htmlToAppend;
            private readonly Stream response;

            /// <summary>
            ///     Initializes a new instance of the <see cref="AddUiToolsFilter" /> class.
            /// </summary>
            /// <param name="response">The response.</param>
            /// <param name="htmlToAppend">The HTML to append.</param>
            public AddUiToolsFilter(Stream response, string htmlToAppend)
            {
                this.response = response;
                this.htmlToAppend = htmlToAppend;
            }

            /// <summary>
            ///     Writes a block of bytes to the current stream using data read from a buffer.
            /// </summary>
            /// <param name="buffer">The buffer to write data from.</param>
            /// <param name="offset">
            ///     The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the
            ///     current stream.
            /// </param>
            /// <param name="count">The maximum number of bytes to write.</param>
            public override void Write(byte[] buffer, int offset, int count)
            {
                var html = Encoding.UTF8.GetString(buffer);
                html = html.Replace("</body>", this.htmlToAppend + "</body>");
                buffer = Encoding.UTF8.GetBytes(html);
                this.response.Write(buffer, offset, buffer.Length);
            }
        }
    }
}