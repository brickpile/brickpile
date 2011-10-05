using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Case", ControllerType = typeof(CaseController))]
    public class Case : BaseEditorial { }
}