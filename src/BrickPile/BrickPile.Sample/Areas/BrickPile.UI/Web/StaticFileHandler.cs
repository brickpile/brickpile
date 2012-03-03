using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Hosting;
using BrickPile.UI.Web.Hosting;

namespace BrickPile.UI.Web {
    public class StaticFileHandler : IHttpHandler {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable {
            get { return true; }
        }

        //public string PublicLink {
        //    get { return ConfigurationManager.AppSettings["DropboxPublicLink"]; }
        //}

        //public int BypassFileSize {
        //    get { return int.Parse(ConfigurationManager.AppSettings["BypassFileSize"] ?? "0"); }
        //}

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context) {

            var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(context.Request.FilePath) as AmazonS3VirtualFile;

            //if (virtualFile == null) {
            //    throw new HttpException(404, "File not found");
            //}
            ////if (virtualFile.Size > 1024 * BypassFileSize) {
            ////    context.Response.Redirect(string.Format(PublicLink, virtualFile.MetaData.Path.Remove(0, 8)), true);
            ////}

            //var lastWriteTime = File.GetLastWriteTime( Path.Combine(virtualFile.ThumbnailDirectory,virtualFile.VirtualPath) );
            //var lastModified = new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day,
            //                                lastWriteTime.Hour, lastWriteTime.Minute, lastWriteTime.Second, 0);

            //DateTime now = DateTime.Now;
            //if (lastModified > now) {
            //    lastModified = new DateTime(now.Ticks - (now.Ticks % 0x989680L));
            //}

            //var etag = GenerateETag(lastModified, now);

            //SetCacheParamters(context, MimeMapping.GetMimeMapping(virtualFile.VirtualPath),
            //                  virtualFile.LocalPath, lastModified, etag);

            //using (Stream stream = virtualFile.Open()) {
            //    stream.CopyTo(context.Response.OutputStream);
            //}

            //return File.OpenRead(context.Request.FilePath);
            context.Response.ContentType = "image/jpg";

            using (Stream stream = File.OpenRead(@"D:\Projects\Git\Stormbreaker\src\BrickPile\BrickPile.Sample\" + context.Request.FilePath.TrimStart(new[] { '/' }))) {
                stream.CopyTo(context.Response.OutputStream);
            }

        }
        /// <summary>
        /// Sets the cache paramters.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="localPath">The local path.</param>
        /// <param name="lastModified">The last modified.</param>
        /// <param name="etag">The etag.</param>
        private void SetCacheParamters(HttpContext context, string mimeType, string localPath, DateTime lastModified, string etag) {
            context.Response.ContentType = mimeType;
            context.Response.AddFileDependency(localPath);
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(3.0));
            context.Response.Cache.SetLastModified(lastModified);
            context.Response.Cache.SetETag(etag);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
        }
        /// <summary>
        /// Generates the E tag.
        /// </summary>
        /// <param name="lastModified">The last modified.</param>
        /// <param name="now">The now.</param>
        /// <returns></returns>
        private static string GenerateETag(DateTime lastModified, DateTime now) {
            long num = lastModified.ToFileTime();
            long num2 = now.ToFileTime();
            string str = num.ToString("X8", CultureInfo.InvariantCulture);
            if ((num2 - num) <= 0x1c9c380L) {
                return ("W/\"" + str + "\"");
            }
            return ("\"" + str + "\"");
        }
    }
}