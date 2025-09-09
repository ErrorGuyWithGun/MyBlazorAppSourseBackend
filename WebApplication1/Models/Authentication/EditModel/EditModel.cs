namespace WebApplication1.Models.Authentication.EditModel
{
    public class EditModel
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }
        public string? Role { get; set; } 

        public bool IsActive { get; set; }
    }
}
