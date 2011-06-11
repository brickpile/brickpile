using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Domain.Models;

namespace BrickPile.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageModel _model;
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(_model);
        }
        public HomeController(IPageModel model) {
            _model = model;
        }
    }
}
