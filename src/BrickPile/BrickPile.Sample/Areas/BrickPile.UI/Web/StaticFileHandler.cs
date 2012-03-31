using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
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
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context) {

            // get the correct provider
            var provider = HostingEnvironment.VirtualPathProvider as AmazonS3VirtualPathProvider;

            // abort if the provider is null
            if (provider == null) {
                throw new Exception("Provider does not exist");
            }
            
            // get the requested file name
            var localFileName = VirtualPathUtility.GetFileName(context.Request.FilePath);

            if (localFileName == null) return;

            // parse width and height of the image
            var regex = new Regex(@"(?<width>\d+)(?:[_]{1})(?<height>\d+)(?=[.])");
            if (!regex.IsMatch(context.Request.FilePath)) {
                throw new Exception("The filename is not well formed, should be name_width_height.extension");
            }

            var match = regex.Match(context.Request.FilePath);

            var width = match.Groups["width"].Value;
            var height = match.Groups["height"].Value;

            regex = new Regex(@"_[\d]+[\d]+(?=[.]?)");
            if(!regex.IsMatch(localFileName)) return;

            var matches = regex.Matches(localFileName);

            // parse the incoming file path
            var virtualPath = provider.VirtualPathRoot + localFileName.Replace(matches[0].Value, null).Replace(matches[1].Value, null);

            // get the file from amazon s3 in format /s3/File.jpg
            var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(virtualPath) as AmazonS3VirtualFile;

            // abort and send not found if file does not exist
            if (virtualFile == null) {
                throw new HttpException(404, "File not found");
            }

            // combine the directory path
            string directory = Path.Combine(provider.LocalPath, virtualFile.Etag.Replace("\"", ""));

            // get the local path for the resized image
            var localPath = Path.Combine(directory, localFileName);

            if (!File.Exists(localPath)) {

                // get the path for the temporary file
                var tmpFile = Path.Combine(directory,
                                           Path.ChangeExtension(
                                               context.Request.FilePath.Replace(provider.VirtualPathRoot, null),
                                               "tmp"));

                // exit with the correct http exception if file is not found
                if (!File.Exists(tmpFile)) {
                    throw new HttpException(404, "File not found");
                }

                // delete the temp file and log any exception
                try {
                    File.Delete(tmpFile);
                } catch (IOException ex) {
                    Debug.WriteLine(ex.Message);
                }

                // download and convert the requested image to the specified size
                // TODO: cache the original image so we can us it the next time another size is requested

                if (!File.Exists(Path.Combine(directory, virtualPath.Replace(provider.VirtualPathRoot,null)))) {
                    using (var ms = new MemoryStream()) {
                        virtualFile.Open().CopyTo(ms);
                        File.WriteAllBytes(
                            Path.Combine(directory, virtualPath.Replace(provider.VirtualPathRoot, null)),
                            ms.ToArray());
                    }
                }

                // resize the image
                var image = new WebImage(Path.Combine(directory, virtualPath.Replace(provider.VirtualPathRoot, null)))
                    .Resize(int.Parse(width) + 1, int.Parse(height) + 1,true,true)
                    .Crop(1, 1);
                image.Save(localPath);
            }

            var lastWriteTime = File.GetLastWriteTime(localPath);
            var lastModified = new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day,
                                            lastWriteTime.Hour, lastWriteTime.Minute, lastWriteTime.Second, 0);

            DateTime now = DateTime.Now;
            if (lastModified > now) {
                lastModified = new DateTime(now.Ticks - (now.Ticks%0x989680L));
            }

            var etag = GenerateETag(lastModified, now);

            SetCacheParamters(context, MimeMapping.GetMimeMapping(localPath),
                              localPath, lastModified, etag);

            using (Stream stream = File.OpenRead(localPath)) {
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