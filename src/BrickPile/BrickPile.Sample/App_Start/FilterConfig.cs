using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web.Mvc;

namespace BrickPile.Sample.App_Start {
    public class FilterConfig {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new SomeHtmlAttribute());
        }
    }

    //public class SomeHtmlAttribute : ActionFilterAttribute {
    //    public override void OnActionExecuting(ActionExecutingContext filterContext) {
    //        filterContext.HttpContext.Response.Filter = new SomeHtmlFilter(filterContext.HttpContext.Response.Filter);
    //        base.OnActionExecuting(filterContext);
    //    }
    //}

    //public class SomeHtmlFilter : MemoryStream {
    //    private readonly Stream _outputStream;
    //    public SomeHtmlFilter(Stream outputStream) {
    //        _outputStream = outputStream;
    //    }

    //    public override void Write(byte[] buffer, int offset, int count) {

    //        var html = Encoding.UTF8.GetString(buffer);

    //        _outputStream.Write(buffer, 0, buffer.Length);

    //    }

    //    public override void Close() {
                            
    //        var buffer = Encoding.UTF8.GetBytes( "<a href=\"#\">Dont click me</a>" );
    //        _outputStream.Write(buffer, 0, buffer.Length);
    //        base.Close();
    //    }
    //}

}