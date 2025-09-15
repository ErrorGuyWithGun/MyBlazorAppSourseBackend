using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Inventory
{
    public class ApiTokenModel
    {
        public Guid Id { get; set; }
        
        [Required]
        public Guid InventoryId { get; set; }
        
        [Required]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ExpiresAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public string CreatedByUserId { get; set; } = string.Empty;
        
        // Navigation property
        public InventoryModel? Inventory { get; set; }
    }
}

