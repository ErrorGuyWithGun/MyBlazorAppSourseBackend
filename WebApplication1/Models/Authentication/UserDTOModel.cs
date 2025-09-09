namespace WebApplication1.Models.Authentication
{
    public class UserDTOModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public string? Role { get; set; }
    }
}
