namespace WebApplication1.Models.Inventory
{
    public class ItemModel
    {
        public Guid Id { get; set; }
        public Guid inventoryId { get; set; }

        public string? Title { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }

        public InventoryModel? Inventory { get; set; }
        public ICollection<ItemTagModel> ItemTags { get; set; } = new List<ItemTagModel>();
    }
}
