namespace WebApplication1.Models.Authentication
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public string? Role;

    }
}
