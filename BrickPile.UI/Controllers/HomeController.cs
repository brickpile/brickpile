using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.UI.Models;

namespace BrickPile.UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(Home currentContent)
        {
            return View(currentContent);
        }

    }
}
