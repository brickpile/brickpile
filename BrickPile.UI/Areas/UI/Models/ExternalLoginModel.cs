using System.ComponentModel.DataAnnotations;

namespace BrickPile.UI.Areas.UI.Models
{
    public class ExternalLoginModel
    {
        [Required]
        public string ProviderName
        {
            get;
            set;
        }

        [Required]
        public string UserIdentifier
        {
            get;
            set;
        }
    }
}