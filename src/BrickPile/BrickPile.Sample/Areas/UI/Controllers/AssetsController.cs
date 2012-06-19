using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using BrickPile.FileSystem.AmazonS3.Common;
using BrickPile.FileSystem.AmazonS3.Hosting;

namespace BrickPile.UI.Controllers {
    /// <summary>
    /// 
    /// </summary>
    public class AssetsController : Controller {

        /// <summary>
        /// Indexes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult Index(string path) {

            if(string.IsNullOrEmpty(path)) {
                path = ((AmazonS3VirtualPathProvider)HostingEnvironment.VirtualPathProvider).VirtualPathRoot;
            }

            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path) as AmazonS3VirtualDirectory;

            return new JsonResult()
            {
                Data = new
                {
                    Files = from file in directory.Files as IEnumerable<AmazonS3VirtualFile> select new
                    {
                        Name = file.Name,
                        VirtualPath = file.VirtualPath,
                        Url = file.Url,
                        Etag = file.Etag,
                        LocalPath = file.LocalPath,
                        Thumbnail = Url.Image(file).Resize(110,100).ToString()

                    },
                    Directories = from dir in directory.Directories as IEnumerable<AmazonS3VirtualDirectory> select new
                    {
                        dir.Name,
                        dir.VirtualPath,
                    },
                    Parent = directory.Parent != null ? 
                    new
                    {
                      directory.Parent.Name,
                      directory.Parent.VirtualPath
                    } : null
                },JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult GetDirectory(string path) {
            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path) as AmazonS3VirtualDirectory;
            return PartialView("Directory", directory);
        }

        /// <summary>
        /// Gets the thumbnail URL.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult GetThumbnailUrl(string path) {
            var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(path) as AmazonS3VirtualFile;
            
            var url = Url.Image(virtualFile).Resize(60, 38).ToString();
            return Content(url);
        }
    }
}
