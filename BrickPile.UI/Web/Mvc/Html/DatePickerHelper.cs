using System.Threading;
using System.Web.Mvc;


namespace BrickPile.UI.Web.Mvc.Html {
    public static class DatePickerHelper {
        public static string ConvertDateFormat(this HtmlHelper html) {
            return ConvertDateFormat(html,
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }
        public static string ConvertDateFormat(this HtmlHelper html, string format) {            
            var currentFormat = format;

            // Convert the date
            currentFormat = currentFormat.Replace("dddd", "DD");
            currentFormat = currentFormat.Replace("ddd", "D");

            // Convert month
            if (currentFormat.Contains("MMMM")) {
                currentFormat = currentFormat.Replace("MMMM", "MM");
            }
            else if (currentFormat.Contains("MMM")) {
                currentFormat = currentFormat.Replace("MMM", "M");
            }
            else if (currentFormat.Contains("MM")) {
                currentFormat = currentFormat.Replace("MM", "mm");
            }
            else {
                currentFormat = currentFormat.Replace("M", "m");
            }

            // Convert year
            currentFormat = currentFormat.Contains("yyyy") ?
            currentFormat.Replace("yyyy", "yy") : currentFormat.Replace("yy", "y");

            return currentFormat;
        }
        public static string ConvertTimeFormat(this HtmlHelper html) {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern.Replace("tt","TT");
        }
        public static string ConvertShortTimeFormat(this HtmlHelper html) {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt", "TT"); ;
        }        
    }
}