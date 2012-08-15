using BrickPile.Domain;
using BrickPile.Sample.Controllers;


namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageType(Name = "Contact", ControllerType = typeof(ContactController))]
    public class Contact : BaseModel { }
}
