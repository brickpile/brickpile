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
            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path);
            return Json(directory.Files, JsonRequestBehavior.AllowGet);
            //return PartialView(directory);
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
            
            var url = Url.Image(virtualFile).Resize(60, 45).ToString();
            return Content(url);
        }
    }
}
