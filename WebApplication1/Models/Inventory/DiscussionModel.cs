namespace WebApplication1.Models.Inventory
{
    public class DiscussionModel
    {
        public Guid Id { get; set; }
        public Guid inventoryId { get; set; }
        public string? userId { get; set; }
        public string? Text { get; set; }
    }
}
