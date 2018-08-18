using System.ComponentModel.DataAnnotations;

namespace Eshop.Models
{
    public class RenameView
    {
        public string OldName { get; set; }
        [Required]
        public string NewName { get; set; }
        public int ItemId { get; set; }
        public string ActionName { get; set; }

        public RenameView(string oldName, int itemId, string actionName)
        {
            OldName = oldName;
            ItemId = itemId;
            ActionName = actionName;

        }

        public RenameView()
        {
        }
    }

}