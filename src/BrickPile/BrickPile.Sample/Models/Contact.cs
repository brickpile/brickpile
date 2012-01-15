using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageModel(Name = "Contact us", ControllerType = typeof(ContactController))]
    public class Contact : BaseEditorial {
        
    }
}