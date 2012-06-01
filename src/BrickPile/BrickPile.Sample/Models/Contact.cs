using BrickPile.Domain;
using BrickPile.Sample.App_LocalResources;
using BrickPile.Sample.Controllers;


namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageType(Name = "Contact", ControllerType = typeof(ContactController), ResourceType = typeof(Resource))]
    public class Contact : BaseEditorial { }
}
