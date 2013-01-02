using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Http;
using BrickPile.Core.Hosting;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Domain.Models;
using Raven.Client;
using Image = BrickPile.Domain.Models.Image;

namespace BrickPile.UI.Controllers {
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    public class AssetController : ApiController {

        private HttpRequest _request = HttpContext.Current.Request;

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Asset> Get() {

            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();
            return session.Query<Asset, AllAssets>()
                .OrderByDescending(x => x.DateUploaded);

        }
        /// <summary>
        /// Gets the specified recent.
        /// </summary>
        /// <param name="recent">The recent.</param>
        /// <returns></returns>
        public IQueryable<Asset> Get(int recent) {
            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();
            return session.Query<Asset, AllAssets>()
                .Where(x => x.DateUploaded > DateTime.Now.AddHours(-48))
                .OrderByDescending(x => x.DateUploaded);
        }
        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IQueryable<Asset> Get(string type) {
            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();
            switch (type) {
                case "image":
                    return  session.Query<Image>().OrderByDescending(x => x.DateUploaded);
                case "video":
                    return session.Query<Video>().OrderByDescending(x => x.DateUploaded);
                case "audio":
                    return session.Query<Audio>().OrderByDescending(x => x.DateUploaded);
                case "document":
                    return session.Query<Document>().OrderByDescending(x => x.DateUploaded);
            }
            return Get();
        }

        public void Delete(string id) {
            // Abort if the provider does not exist
            var virtualPathProvider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;
            if(virtualPathProvider == null)
                return;

            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();
            var item = session.Load<Asset>(id);

            var asset = virtualPathProvider.GetFile(item.VirtualPath) as CommonVirtualFile;
            
            var mediumThumbnail = virtualPathProvider.GetFile(item.Thumbnails.Medium.VirtualPath) as CommonVirtualFile;

            if(asset != null) {
                asset.Delete();
            }
            if (mediumThumbnail != null) {
                mediumThumbnail.Delete();
            }
            session.Delete(item);
            session.SaveChanges();
        }
        /// <summary>
        /// Posts this instance.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Asset>> Post() {

            if (!Request.Content.IsMimeMultipartContent()) {

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));

            }

            var task = Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider()).ContinueWith(t =>
                {

                    var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();

                    var virtualPathProvider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;

                    if (t.IsFaulted || t.IsCanceled || virtualPathProvider == null) {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    var asset = t.Result.Contents.Select(httpContent =>
                    {

                        var stream = httpContent.ReadAsStreamAsync().Result;
                        var length = stream.Length;

                        var virtualDirectory = virtualPathProvider.GetDirectory(virtualPathProvider.VirtualPathRoot) as CommonVirtualDirectory;

                        var name = !string.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName)
                                           ? httpContent.Headers.ContentDisposition.FileName
                                           : "NoName";

                        name = name.Replace("\"", string.Empty);

                        var uniqueFileName = Guid.NewGuid().ToString("n");

                        var virtualFile = virtualDirectory.CreateFile(string.Format("{0}{1}",uniqueFileName, VirtualPathUtility.GetExtension(name)));

                        using (var s = virtualFile.Open(FileMode.Create)) {
                            var bytesInStream = new byte[stream.Length];
                            stream.Read(bytesInStream, 0, bytesInStream.Length);
                            s.Write(bytesInStream, 0, bytesInStream.Length);
                        }

                        Asset file;
                        if (httpContent.Headers.ContentType.MediaType.Contains("image")) {

                            var mediumThumbnail = new WebImage(stream).Resize(111, 101).Crop(1, 1);

                            var virtualMediumThumbnail =
                                virtualDirectory.CreateFile(string.Format("{0}{1}", uniqueFileName + "_medium",
                                                                          VirtualPathUtility.GetExtension(name)));

                            using (var s = virtualMediumThumbnail.Open(FileMode.Create)) {
                                s.Write(mediumThumbnail.GetBytes(), 0, mediumThumbnail.GetBytes().Length);
                            }

                            file = new Image();
                            
                            //((Image)file).Width = 1024;
                            //((Image)file).Height = 768;

                            file.Thumbnails.Small.Height = mediumThumbnail.Height;
                            file.Thumbnails.Small.Width = mediumThumbnail.Width;
                            file.Thumbnails.Small.Url = virtualMediumThumbnail.Url;
                            file.Thumbnails.Small.VirtualPath = virtualMediumThumbnail.VirtualPath;

                            file.Thumbnails.Medium.Height = mediumThumbnail.Height;
                            file.Thumbnails.Medium.Width = mediumThumbnail.Width;
                            file.Thumbnails.Medium.Url = virtualMediumThumbnail.Url;
                            file.Thumbnails.Medium.VirtualPath = virtualMediumThumbnail.VirtualPath;

                        }
                        else if (httpContent.Headers.ContentType.MediaType.Contains("video")) {
                            
                            file = new Video();

                            file.Thumbnails.Small.Height = 38;
                            file.Thumbnails.Small.Width = 60;
                            file.Thumbnails.Small.Url = "http://placehold.it/60x38";

                            file.Thumbnails.Medium.Height = 110;
                            file.Thumbnails.Medium.Width = 100;
                            file.Thumbnails.Medium.Url = "http://placehold.it/110x100";

                        }
                        else if (httpContent.Headers.ContentType.MediaType.Contains("audio")) {
                            file = new Audio();
                            file.Thumbnails.Small.Height = 38;
                            file.Thumbnails.Small.Width = 60;
                            file.Thumbnails.Small.Url = "http://placehold.it/60x38";

                            file.Thumbnails.Medium.Height = 110;
                            file.Thumbnails.Medium.Width = 100;
                            file.Thumbnails.Medium.Url = "http://placehold.it/110x100";

                        } else {
                            file = new Document();
                            file.Thumbnails.Small.Height = 38;
                            file.Thumbnails.Small.Width = 60;
                            file.Thumbnails.Small.Url = "http://placehold.it/60x38";

                            file.Thumbnails.Medium.Height = 110;
                            file.Thumbnails.Medium.Width = 100;
                            file.Thumbnails.Medium.Url = "http://placehold.it/110x100";

                        }

                        file.Name = name;
                        file.ContentType = httpContent.Headers.ContentType.MediaType;
                        file.ContentLength = length;
                        file.DateUploaded = DateTime.Now;
                        file.VirtualPath = virtualFile.VirtualPath;
                        file.Url = virtualFile.Url;

                        session.Store(file);
                        session.SaveChanges();

                        return file;
                    });

                    return asset;
                });
            
            return task;
        }
    }
}

