using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; } 
        public string? Password { get; set; }

    }
}
