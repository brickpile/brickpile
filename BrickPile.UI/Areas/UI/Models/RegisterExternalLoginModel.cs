using System.ComponentModel.DataAnnotations;

namespace BrickPile.UI.Areas.UI.Models
{
    public class RegisterExternalLoginModel : ExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName
        {
            get;
            set;
        }
    }
}