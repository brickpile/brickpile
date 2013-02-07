using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrickPile.Domain;
using BrickPile.Domain.Models;

namespace BrickPile.Docs.usage {
    #region Localize page model name
    //[PageType(Name = "Standard page", ControllerType = typeof(SampleController))]
    //public class StandardPage : PageModel {
    //    public string Heading { get; set; }
    //    [DataType(DataType.MultilineText)]
    //    public string MainIntro { get; set; }
    //    [DataType(DataType.Html)]
    //    public string MainBody { get; set; }
    //}
    #endregion
    public class SampleController : Controller {}
}