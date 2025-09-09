using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Authentication.ResetPassword
{
    public class ResetPasswordModel
    {
        public string? Password { get; set; }
        public string? Email { get;set; }
        public string? Token { get; set; }
    }
}
