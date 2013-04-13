namespace BrickPile.UI.Areas.UI.Models
{
    public class ManageModel : LocalPasswordModel
    {
        public string UserName
        {
            get;
            set;
        }

        public string StatusMessage
        {
            get;
            set;
        }

        public bool HasLocalPassword
        {
            get;
            set;
        }
    }
}