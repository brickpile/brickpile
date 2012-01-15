using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using BrickPile.UI.Web.Hosting;

namespace BrickPile.UI.Controllers {
    public class AssetController : Controller {

        public ActionResult Index(string path = "Images/") {

            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(Path.Combine(((AmazonS3VirtualPathProvider)HostingEnvironment.VirtualPathProvider).VirtualPathRoot,path));

            return PartialView(directory.Files);

        }

    }
}
