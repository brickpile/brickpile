using BrickPile.Sample.Models;
using BrickPile.Services;
using BrickPile.UI;

namespace BrickPile.Sample.Web.ViewModels {
    public class HomeViewModel : BaseViewModel<Home> {
        private readonly Home _model;
        private readonly IPageService _pageService;

        public BaseModel MainTeaser { get; set; }
        public BaseEditorial TeaserOne { get; set; }
        public BaseEditorial TeaserTwo { get; set; }
        public BaseEditorial TeaserThree { get; set; }
        public BaseEditorial TeaserFour { get; set; }

        /// <summary>
        /// Gets the main teaser.
        /// </summary>
        //public BaseModel MainTeaser {
        //    get {
        //        return _pageService.SingleOrDefault<BaseModel>(x => x.Id == _model.MainTeaserLink.Id);
        //    }
        //}
        ///// <summary>
        ///// Gets the teaser one.
        ///// </summary>
        //public BaseEditorial TeaserOne {
        //    get {
        //        return _pageService.SingleOrDefault<BaseEditorial>(x => x.Id == _model.TeaserOne.Id);
        //    }
        //}
        ///// <summary>
        ///// Gets the teaser two.
        ///// </summary>
        //public BaseEditorial TeaserTwo {
        //    get {
        //        return _pageService.SingleOrDefault<BaseEditorial>(x => x.Id == _model.TeaserTwo.Id);
        //    }
        //}
        ///// <summary>
        ///// Gets the teaser three.
        ///// </summary>
        //public BaseEditorial TeaserThree {
        //    get {
        //        return _pageService.SingleOrDefault<BaseEditorial>(x => x.Id == _model.TeaserThree.Id);
        //    }
        //}
        ///// <summary>
        ///// Gets the teaser four.
        ///// </summary>
        //public BaseEditorial TeaserFour {
        //    get {
        //        return _pageService.SingleOrDefault<BaseEditorial>(x => x.Id == _model.TeaserFour.Id);
        //    }
        //}
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="pageRepository">The page repository.</param>
        public HomeViewModel(Home model, IStructureInfo structureInfo, IPageService pageRepository)
            : base(model, structureInfo) {
            _model = model;
            _pageService = pageRepository;
        }
    }
}