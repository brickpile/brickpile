using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Samples.Models;

namespace BrickPile.Samples.Controllers
{
    public class DaModelController : Controller
    {
        // GET: DaModel
        public ActionResult Index(DaModel currentPage)
        {
            return View(currentPage);
        }
    }
}