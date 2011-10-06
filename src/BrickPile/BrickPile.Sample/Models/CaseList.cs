using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Case list", ControllerType = typeof(CaseListController))]
    public class CaseList : BaseModel {
        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        public virtual string Heading { get; set; }
        /// <summary>
        /// Gets or sets the main body.
        /// </summary>
        /// <value>
        /// The main body.
        /// </value>
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }
    }
}