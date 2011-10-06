using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageModel(Name = "Case", ControllerType = typeof(CaseController))]
    public class Case : BaseEditorial { }
}