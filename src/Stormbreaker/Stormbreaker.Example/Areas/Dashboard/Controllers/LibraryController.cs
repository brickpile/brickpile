using System;
using System.Diagnostics;
using System.Web.Mvc;
using LitS3;
using Stormbreaker.Repositories;

namespace Dashboard.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IDropboxRepository _dropboxRepository;

        public ActionResult Index() {

            //var client = new DropboxClient("ksmxt4l46011hxt", "ccri19akqme30eh");

            //call the functions you want from the client
            //UserLogin userLogin = client.Login("marcus@meridium.se", "pkrMum");

            //rootDetails.Contents is a list of the files/folders in the root

            //var file = client.GetFile("binero.txt");

            //var vf = (DropboxVirtualFile) HostingEnvironment.VirtualPathProvider.GetFile("~/Dropbox/binero.txt");
            
            //var directories = vd.Directories;
            
            //var rootDetails = client.GetMetaData("/");

            //return View(rootDetails.Contents);
            return View();

        }

        public LibraryController(IDropboxRepository dropboxRepository) {
            _dropboxRepository = dropboxRepository;
        }
    }
}
