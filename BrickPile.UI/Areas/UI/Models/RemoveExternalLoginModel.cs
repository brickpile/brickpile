using System.Collections.Generic;

namespace BrickPile.UI.Areas.UI.Models
{
    public class RemoveExternalLoginModel : ExternalLoginModel
    {
        public IEnumerable<ExternalLoginModel> ExternalLogins
        {
            get;
            set;
        }

        public bool ShowRemoveButton
        {
            get;
            set;
        }
    }
}