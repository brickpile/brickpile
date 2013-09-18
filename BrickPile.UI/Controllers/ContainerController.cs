using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Samples.Models;

namespace BrickPile.Samples.Controllers
{
    public class ContainerController : Controller
    {
        //
        // GET: /Container/

        public ActionResult Index(Container currentPage)
        {
            return View(currentPage);
        }

    }
}
