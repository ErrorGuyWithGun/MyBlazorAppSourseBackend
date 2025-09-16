using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Inventory
{
    public class ItemTagModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid ItemId { get; set; }
        
        [Required]
        public string TagId { get; set; } = string.Empty;

        public ItemModel? Item { get; set; }
        public TagsModel? Tag { get; set; }
    }
}
