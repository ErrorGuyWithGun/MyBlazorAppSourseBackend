using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Authentication.EditModel;

namespace WebApplication1.Controllers
{
    [EnableCors("AllowLocalhost7170")]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
  
     
        public AdminController(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }


        [HttpGet("GetAllUser")]

        public IActionResult GetAll()
        {
            return Ok(_userManager.Users.ToList());
        }

        [HttpGet("GetUserCount")]
        public IActionResult GetUserCount()
        {
            return Ok(_userManager.Users.Count());
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(int startIndex, int endIndex) 
        {
            return Ok(_userManager.Users.Skip(startIndex).Take(endIndex - startIndex).ToList());
        }
        [HttpGet ("GetUserRole")]
        public async Task<string> GetUserRole(string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            var role = await _userManager.GetRolesAsync(user);
            return role[0];
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] EditModel user)
        {
            var thisUser = await _userManager.FindByEmailAsync(user.Email);
            if (thisUser != null) 
            {
                thisUser.UserName = user.UserName;
                thisUser.IsActive = user.IsActive;
                var role = await _userManager.GetRolesAsync(thisUser);
                await _userManager.RemoveFromRoleAsync(thisUser, role[0]);
                await _userManager.AddToRoleAsync(thisUser, user.Role);

                return StatusCode(StatusCodes.Status200OK, 
                    new Response { Status = "Success", Message = "User redacted" });
            }
            return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "User not found" });

        }

        [HttpDelete("DeleteUserByEmail/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null) 
            {
                await _userManager.DeleteAsync(user);
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "User was been delete" });
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Error", Message = "User not found" });
        }
    }
}
