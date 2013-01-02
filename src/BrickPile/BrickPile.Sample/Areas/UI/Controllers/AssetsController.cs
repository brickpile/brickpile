using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using BrickPile.Core.Hosting;
using BrickPile.FileSystem.AmazonS3.Common;

namespace BrickPile.UI.Controllers {
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AssetsController : Controller {

        /// <summary>
        /// Indexes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public ActionResult Index(string path) {

            if(string.IsNullOrEmpty(path)) {
                var provider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;
                if(provider == null) {
                    return new EmptyResult();
                }
                path = provider.VirtualPathRoot;
            }

            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path) as CommonVirtualDirectory;

            return new JsonResult
            {
                Data = new
                {
                    Files = from file in directory.Files as IEnumerable<CommonVirtualFile>
                            select new
                    {
                        Name = file.Name,
                        VirtualPath = file.VirtualPath,
                        Url = file.Url,
                        LocalPath = file.LocalPath,
                        Thumbnail = Url.Image(file.VirtualPath).Resize(110,100).ToString()

                    },
                    Directories = from dir in directory.Directories as IEnumerable<CommonVirtualDirectory>
                                  select new
                    {
                        dir.Name,
                        dir.VirtualPath                        
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
            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(path) as CommonVirtualDirectory;
            return PartialView("Directory", directory);
        }

        /// <summary>
        /// Gets the thumbnail URL.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        //public ActionResult GetThumbnailUrl(string path) {
        //    var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(path) as CommonVirtualFile;
        //    var url = virtualFile == null ? "http://placehild.it/60x38" : Url.Image(virtualFile.VirtualPath).Resize(60, 38).ToString();
        //    return Content(url);
        //}
        /// <summary>
        /// Adds the directory.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="directoryName">Name of the directory.</param>
        /// <returns></returns>
        //public ActionResult CreateDirectory(string virtualPath, string directoryName) {
        //    if(string.IsNullOrEmpty(virtualPath)) {
        //        var provider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;
        //        virtualPath = provider.VirtualPathRoot;
        //    }
        //    var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualPath) as CommonVirtualDirectory;
        //    var dir = directory.CreateDirectory(directoryName);

        //    return new JsonResult
        //    {
        //        Data = new
        //        {
        //            name = dir.Name,
        //            virtualPath = dir.VirtualPath
        //        },JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };

        //}
        //[HttpPost]
        //public ActionResult Upload(FormCollection form) {

        //    return Content(Request.Files.Count.ToString());

        //}
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <returns></returns>
        //public ActionResult DeleteDirectory(string virtualPath) {

        //    if (string.IsNullOrEmpty(virtualPath)) {
        //        var provider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;
        //        virtualPath = provider.VirtualPathRoot;
        //    }
        //    var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualPath) as CommonVirtualDirectory;
        //    directory.Delete();
        //    return new EmptyResult();
        //}
    }
}
