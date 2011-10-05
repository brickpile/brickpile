using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Case list", ControllerType = typeof(CaseListController))]
    public class CaseList : BaseModel {
        public virtual string Heading { get; set; }
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }
    }
}