using System.Web.Hosting;
using System.Web.Mvc;
using BrickPile.UI.Common;

namespace BrickPile.UI.Controllers {
    /// <summary>
    /// 
    /// </summary>
    public class AssetController : Controller {

        /// <summary>
        /// Indexes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult Index(string path = "/s3/") {
            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path);
            return PartialView(directory);
        }
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult GetDirectory(string path) {
            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path);
            return PartialView("Directory", directory);
        }

        /// <summary>
        /// Gets the thumbnail URL.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult GetThumbnailUrl(string path) {
            var url = Url.Image(path).Resize(60, 45).ToString();

            return Content(url);
        }
    }
}
