using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models.Authentication
{
    public class User:IdentityUser
    {
        public bool IsActive { get; set; }
        public string? FacebookId { get; set; }
    }
}
