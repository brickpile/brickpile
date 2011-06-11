using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.Web.ViewModels;
using BrickPile.Services;
using BrickPile.UI;

namespace BrickPile.Sample.Controllers {
    public class HomeController : Controller {
        private readonly Home _model;
        private readonly IStructureInfo _structureInfo;
        private readonly IPageService _pageService;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            var teasers = _pageService.Load<IPageModel>(new[]
                                                  {
                                                      _model.TeaserOne.Id, _model.TeaserTwo.Id, _model.TeaserThree.Id,
                                                      _model.TeaserFour.Id, _model.MainTeaserLink.Id
                                                  });

            var model = new HomeViewModel(_model, _structureInfo, _pageService)
                            {
                                TeaserOne = (BaseEditorial)teasers[0],
                                TeaserTwo = (BaseEditorial)teasers[0],
                                TeaserThree = (BaseEditorial)teasers[0],
                                TeaserFour = (BaseEditorial)teasers[0],
                                MainTeaser = (BaseModel)teasers[1],
                                Class = "home"
                            };
            return View(model);

            //return View(new HomeViewModel(_model, _structureInfo, _pageService) { Class = "home" });
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="pageService">The page service.</param>
        public HomeController(IPageModel model, IStructureInfo structureInfo, IPageService pageService) {
            _model = model as Home;
            _structureInfo = structureInfo;
            _pageService = pageService;
        }
    }
}
