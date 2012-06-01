using System.Web.Mvc;
using BrickPile.Docs.Resources;
using BrickPile.Domain;
using BrickPile.Domain.Models;

namespace BrickPile.Docs.usage {
    #region Localize page model name
    [PageType(Name = "MyModel", ControllerType = typeof(SampleController), ResourceType = typeof(Resource))]
    public class SampleModel : PageModel {
        // properties goes here
    }
    #endregion
    public class SampleController : Controller {}
}