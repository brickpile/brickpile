using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;
using BrickPile.UI.Models;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Data type tests", ControllerType = typeof(PageController))]
    public class DataTypeTest : PageModel {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        [Display(Name = "Link",Prompt = "Specify page name or browse...")]
        public ModelReference Link { get; set; }
    }
}