using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Deserializers;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Hosting;

namespace Dashboard.Controllers {
    [Authorize]
    public class DashboardController : Controller {
        private readonly IPageRepository _repository;
        private UserLogin _userLogin;
        private const int _version = 0;
        private const string _apiKey = "ksmxt4l46011hxt";
        private const string _appsecret = "ccri19akqme30eh";
        private string _token;
        private string _secret;
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/

        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        public DashboardController(IPageRepository repository)
        {
            _repository = repository;
        }


        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        public ActionResult Index() {
            //var provider = new DropboxVirtualPathProvider();

            //VirtualFile file = provider.GetFile("~/binero.txt");

            //_userLogin = Login("marcus@meridium.se", "pkrMum");
            //var bytes = DownloadFile("iPhone intro.pdf");
            //writeByteArrayToFile(bytes, @"c:\iPhone intro.pdf");
            return View();
        }
        public bool writeByteArrayToFile(byte[] buff, string fileName)
        {
            bool response = false;

            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(buff);
                bw.Close(); //Thanks Karlo for pointing out!
                response = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
    }

}
