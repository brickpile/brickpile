using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;


namespace BrickPile.UI.Areas.UI
{
    public static class UrlHelperExtensions
    {
        public static Uri Public(this UrlHelper urlHelper, string relativeUri = "/")
        {
            return Public(urlHelper, new Uri(relativeUri, UriKind.Relative));
        }

        public static Uri Public(this UrlHelper urlHelper, Uri relativeUri)
        {

            var proto = "http";
            IEnumerable<string> headerValues;

            if (urlHelper.Request.Headers.TryGetValues("X-Forwarded-Proto", out headerValues)) {
                proto = headerValues.FirstOrDefault();
            }

            var uriBuilder = new UriBuilder
            {
                Host = urlHelper.Request.RequestUri.Host,
                Path = "/",
                Scheme = proto,
            };

            uriBuilder.Port = urlHelper.Request.RequestUri.Port;

            return new Uri(uriBuilder.Uri, relativeUri);

            //if (relativeUri == null)
            //    return String.Empty;

            //var request = urlHelper.Request;
            ////var request        = urlHelper.RequestContext.HttpContext.Request;
            //string fwdProtocol = request.Headers.
            ////string fwdProtocol = request.Headers["X-Forwarded-Proto"] ?? String.Empty;
            //string path        = relativeUri.ToString();

            //if (request.Url == null)
            //    return relativeUri.ToString();

            //if (relativeUri.IsAbsoluteUri)
            //    path = relativeUri.PathAndQuery;

            //var uriBuilder = new UriBuilder
            //{
            //    Scheme = request.Url.Scheme,
            //    Host   = request.Url.Host,
            //    Port   = 80,
            //    Path   = path,
            //};

            //if (request.IsSecureConnection || fwdProtocol.ToLowerInvariant() == "https")
            //{
            //    uriBuilder.Scheme = "https";
            //    uriBuilder.Port = 443;
            //}
            //else if (request.IsLocal)
            //{
            //    uriBuilder.Port = request.Url.Port;
            //}
            //else if (request.Url.Port == 82)
            //{
            //    uriBuilder.Port = 80;
            //}

            //return new Uri(uriBuilder.Uri, path).AbsoluteUri;
        }
    }
}