using System;
using System.Collections.Generic;

namespace BrickPile.UI.Areas.UI
{
    public static class UriExtensions
    {
        public static Uri AddQueryParameter(this Uri uri, KeyValuePair<string, string> pair)
        {
            return uri.AddQueryParameter(pair.Key, pair.Value);
        }

        public static Uri AddQueryParameter(this Uri uri, string key, string value)
        {
            var builder = new UriBuilder(uri);

            string encodedKey   = Uri.EscapeDataString(key);
            string encodedValue = Uri.EscapeDataString(value);
            string queryString  = String.Format("{0}={1}", encodedKey, encodedValue);

            builder.Query += String.IsNullOrEmpty(builder.Query) ? queryString : "&" + queryString;

            return builder.Uri;
        }
    }
}