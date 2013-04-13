using System.Collections.Generic;

namespace BrickPile.UI.Areas.UI.Models
{
    public class ExternalLoginsListModel
    {
        public IEnumerable<string> ProviderNames
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }
    }
}