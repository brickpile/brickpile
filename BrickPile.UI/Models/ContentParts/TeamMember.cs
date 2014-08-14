using BrickPile.Samples.Models.ContentTypes;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models.ContentParts
{
    public class TeamMember
    {
        public Image Image { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }

        // Social buttons, not editable atm
    }
}