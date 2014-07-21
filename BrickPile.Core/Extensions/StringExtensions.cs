using System.Web;

namespace BrickPile.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static string AddQueryParam(this string source, string key, string value)
        {
            string delim;
            if ((source == null) || !source.Contains("?"))
            {
                delim = "?";
            }
            else if (source.EndsWith("?") || source.EndsWith("&"))
            {
                delim = string.Empty;
            }
            else
            {
                delim = "&";
            }
            return source + delim + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value);
        }
    }
}
