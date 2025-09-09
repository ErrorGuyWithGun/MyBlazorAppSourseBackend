namespace WebApplication1.Models.Inventory
{
    public class InventoryModel
    {
        public Guid Id { get; set; }
        public string? userId { get; set; }
        public string? Name { get; set; }

        public Guid? categoryId { get; set; }
        public bool? isPublic { get; set; }
    }
}
