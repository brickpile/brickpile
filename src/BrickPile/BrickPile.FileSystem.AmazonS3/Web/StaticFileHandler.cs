using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;
using System.Web.Hosting;
using BrickPile.Core.Hosting;

namespace BrickPile.FileSystem.AmazonS3.Web {
    public class StaticFileHandler : IHttpHandler {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable {
            get { return true; }
        }
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context) {

            //// get the correct provider
            var provider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;

            //// abort if the provider is null
            if (provider == null) {
                throw new Exception("Provider does not exist");
            }

            var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(context.Request.FilePath) as CommonVirtualFile;

            //// abort and send not found if file does not exist
            if (virtualFile == null) {
                throw new HttpException(404, "File not found");
            }

            var width = context.Request.QueryString["width"] ?? string.Empty;
            var height = context.Request.QueryString["height"] ?? string.Empty;

            // Resize image
            if (!string.IsNullOrEmpty(width) && !string.IsNullOrEmpty(height)) {
                var hash = CalculateMd5Hash(context.Request.RawUrl);
                WebImage image;
                if (HttpRuntime.Cache[hash] == null) {
                    // resize the image
                    image = new WebImage(virtualFile.Open())
                        .Resize(int.Parse(width) + 1, int.Parse(height) + 1, true, true)
                        .Crop(1, 1);

                    HttpRuntime.Cache.Add(hash, image, null,
                      Cache.NoAbsoluteExpiration,
                      new TimeSpan(0, 60, 0),
                      CacheItemPriority.Default, null);

                } else {
                    image = HttpRuntime.Cache[hash] as WebImage;
                }
                if (image != null) {
                    image.Write();
                    return;
                }
            }

            //var lastWriteTime = File.GetLastWriteTime(virtualFile.LocalPath);
            //var lastModified = new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day,
            //                                lastWriteTime.Hour, lastWriteTime.Minute, lastWriteTime.Second, 0);

            //DateTime now = DateTime.Now;
            //if (lastModified > now) {
            //    lastModified = new DateTime(now.Ticks - (now.Ticks % 0x989680L));
            //}

            //var etag = GenerateETag(lastModified, now);

            //SetCacheParamters(context, MimeMapping.GetMimeMapping(virtualFile.LocalPath),
            //                  virtualFile.LocalPath, lastModified, etag);


            context.Response.ContentType = MimeMapping.GetMimeMapping(virtualFile.VirtualPath);
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(10));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            using(Stream stream = virtualFile.Open()) {
                stream.CopyTo(context.Response.OutputStream);
            }

            //using (Stream stream = File.OpenRead(virtualFile.LocalPath)) {
            //    stream.CopyTo(context.Response.OutputStream);
            //}
        }
        /// <summary>
        /// Calculates the MD5 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string CalculateMd5Hash(string input) {
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
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
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(10));
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
        public static byte[] StreamToByteArray(Stream stream) {
            if (stream is MemoryStream) {
                return ((MemoryStream)stream).ToArray();
            }
            // Jon Skeet's accepted answer 
            return ReadFully(stream);
        }
        public static byte[] ReadFully(Stream input) {
            using (MemoryStream ms = new MemoryStream()) {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        
    }
}