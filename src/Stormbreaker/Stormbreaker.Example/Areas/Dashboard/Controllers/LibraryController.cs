using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class LibraryController : Controller
    {
        public ActionResult Index() {

            //var client = new DropNet.DropNetClient("ksmxt4l46011hxt", "ccri19akqme30eh");

            //call the functions you want from the client
            //UserLogin userLogin = client.Login("marcus@meridium.se", "pkrMum");

            //rootDetails.Contents is a list of the files/folders in the root
            //var rootDetails = userLogin;

            return View();
        }
        public LibraryController() {
        }
    }
}
