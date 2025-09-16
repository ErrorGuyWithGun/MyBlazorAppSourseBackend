using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Inventory
{
    public class TagsModel
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public ICollection<ItemTagModel> ItemTags { get; set; } = new List<ItemTagModel>();
    }
}
