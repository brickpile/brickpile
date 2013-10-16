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
using BrickPile.UI.Web;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.UI.Areas.UI.Controllers {
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    public class AssetController : ApiController {
        private const int PageSize = 50;

        public AssetResponse Get(int page) {
            
            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();

            var response = new AssetResponse();

            RavenQueryStatistics stats;
            response.Assets = Queryable.Skip(session.Query<Asset, AllAssets>()
                          .Statistics(out stats)
                          .OrderByDescending(x => x.DateUploaded), page * PageSize)
                .Take(PageSize).ToArray();

            response.SkippedResults = stats.SkippedResults;
            response.TotalResults = stats.TotalResults;
            response.RegistredProvider = VirtualPathProviderRegistry.RegistratedProviders;
            return response;
        }

        public class AssetResponse {
            public int TotalResults { get; set; }
            public int SkippedResults { get; set; }
            public IEnumerable<Asset> Assets { get; set; }
            public List<VirtualPathProviderRegistry.CommonProviderSettings> RegistredProvider { get; set; }
        }

        /// <summary>
        /// Gets the specified recent.
        /// </summary>
        /// <param name="recent">The recent.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public AssetResponse Get(int page, int recent) {
            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();

            var response = new AssetResponse();

            RavenQueryStatistics stats;
            response.Assets = Queryable.Skip(session.Query<Asset, AllAssets>()
                          .Statistics(out stats)
                          .Where(x => x.DateUploaded > DateTime.Now.AddHours(-48))
                          .OrderByDescending(x => x.DateUploaded), page * PageSize)
                .Take(PageSize).ToArray();

            response.SkippedResults = stats.SkippedResults;
            response.TotalResults = stats.TotalResults;
            response.RegistredProvider = VirtualPathProviderRegistry.RegistratedProviders;
            return response;

        }

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="type">The type.</param>
        /// <param name="virtualDirectory">The virtual directory </param>
        /// <returns></returns>
        public AssetResponse Get(int page, string type,string virtualDirectory=null) {
            switch (type) {
                case "image":
                    return GetResponse<Image>(page, virtualDirectory);
                case "video":
                    return GetResponse<Video>(page, virtualDirectory);
                case "audio":
                    return GetResponse<Audio>(page, virtualDirectory);
                case "document":
                    return GetResponse<Document>(page, virtualDirectory);
                default:
                    return Get(0);
            }
        }
        private AssetResponse GetResponse<T>(int page, string virtualDirectory) where T : Asset {
            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();
            var response = new AssetResponse();
            RavenQueryStatistics stats;
            response.Assets = Queryable.Skip(session.Query<T>()
                .Statistics(out stats)
                .Where(x => x.VirtualPath.StartsWith(virtualDirectory, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(x => x.DateUploaded), page * PageSize)
                .Take(PageSize).ToArray();
            response.SkippedResults = stats.SkippedResults;
            response.TotalResults = stats.TotalResults;
            response.RegistredProvider = VirtualPathProviderRegistry.RegistratedProviders;
            return response;
        }

        public void Delete(string id) {
            // Abort if the provider does not exist
            var virtualPathProvider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;
            if(virtualPathProvider == null) { return; }

            var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();
            var item = session.Load<Asset>(id);

            var asset = virtualPathProvider.GetFile(item.VirtualPath) as CommonVirtualFile;

            if(asset != null) {
                asset.Delete();
            }
            session.Delete(item);
            session.SaveChanges();
        }
        /// <summary>
        /// Posts this instance.
        /// </summary>
        /// <param name="virtualDirectoryPath"></param>
        /// <returns></returns>
        public Task<IEnumerable<Asset>> Post(string virtualDirectoryPath=null) {

            if (!Request.Content.IsMimeMultipartContent()) {

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));

            }

            var task = Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider()).ContinueWith(t =>
                {

                    var session = StructureMap.ObjectFactory.GetInstance<IDocumentSession>();


                    if (t.IsFaulted || t.IsCanceled) {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    var asset = t.Result.Contents.Select(httpContent =>
                    {
                        // Read the stream
                        var stream = httpContent.ReadAsStreamAsync().Result;
                        var length = stream.Length;

                        // Get root directory of the current virtual path provider
                        //TODO Backward compability if virtualDirectoryPath is not posted, remove this after
                        virtualDirectoryPath = virtualDirectoryPath ??
                                               VirtualPathProviderRegistry.RegistratedProviders[0].VirtualPathRoot;


                        var virtualDirectory = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualDirectoryPath) as CommonVirtualDirectory;

                        if (virtualDirectory == null) {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }

                        // Set the name and if not present add some bogus name
                        var name = !string.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName)
                                           ? httpContent.Headers.ContentDisposition.FileName
                                           : "NoName";

                        // Clean up the name
                        name = name.Replace("\"", string.Empty);

                        // Create a new name for the file stored in the vpp
                        var uniqueFileName = Guid.NewGuid().ToString("n");
                        
                        // Create the file in current directory
                        var virtualFile = virtualDirectory.CreateFile(string.Format("{0}{1}",uniqueFileName, VirtualPathUtility.GetExtension(name)));

                        // Write the file to the current storage
                        using (var s = virtualFile.Open(FileMode.Create)) {
                            var bytesInStream = new byte[stream.Length];
                            stream.Read(bytesInStream, 0, bytesInStream.Length);
                            s.Write(bytesInStream, 0, bytesInStream.Length);
                        }

                        Asset file;
                        if (httpContent.Headers.ContentType.MediaType.Contains("image")) {
                            file = new Image();
                            using (var image = System.Drawing.Image.FromStream(stream, false, false)) {
                                ((Image)file).Width = image.Width;
                                ((Image)file).Height = image.Height;
                            }
                            var mediumThumbnail = new WebImage(stream).Resize(111, 101).Crop(1, 1);
                            file.Thumbnail = mediumThumbnail.GetBytes();
                        }
                        else if (httpContent.Headers.ContentType.MediaType.Contains("video")) {
                            var icon = new WebImage(HttpContext.Current.Server.MapPath("~/areas/ui/content/images/document.png"));
                            file = new Video { Thumbnail = icon.GetBytes() };
                        }
                        else if (httpContent.Headers.ContentType.MediaType.Contains("audio")) {
                            var icon = new WebImage(HttpContext.Current.Server.MapPath("~/areas/ui/content/images/document.png"));
                            file = new Video { Thumbnail = icon.GetBytes() };
                        } else {
                            var icon = new WebImage(HttpContext.Current.Server.MapPath("~/areas/ui/content/images/document.png"));
                            file = new Video { Thumbnail = icon.GetBytes() };
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

