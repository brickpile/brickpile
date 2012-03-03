using System.Web.Hosting;
using System.Web.Mvc;

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
        public ActionResult GetDirectory(string path) {
            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path);
            return PartialView("Directory", directory);
            
        }
    }
}
