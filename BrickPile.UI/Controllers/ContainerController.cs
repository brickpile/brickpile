using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Samples.Models;

namespace BrickPile.Samples.Controllers
{
    public class ContainerController : Controller
    {
        //
        // GET: /Container/

        public ActionResult Index(PageModel currentPage, Container currentContent) {
            var viewModel = new ContainerViewModel
            {
                CurrentContent = currentContent,
                CurrentPage = currentPage
            };
            return View(viewModel);
        }

    }
}
