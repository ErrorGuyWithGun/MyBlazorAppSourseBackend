using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Web.Email.Model;
using Web.Email.Services;
using WebApplication1.Models;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Authentication.LogIn;
using WebApplication1.Models.Authentication.ResetPassword;
using WebApplication1.Models.Authentication.SignUp;

namespace WebApplication1.Controllers
{
    [EnableCors("AllowLocalhost7170")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthenticationController
            (UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration,
            IEmailService emailService)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._emailService = emailService;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser registerUser, string role)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(registerUser.Email))
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });

            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }
            User user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Name,
                IsActive = true
            };
            if (await _roleManager.RoleExistsAsync(role)) 
            {

                var result = await _userManager.CreateAsync(user, registerUser.Password);
                await _userManager.AddToRoleAsync(user, role);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message
                    (new string[] { user.Email }, 
                    "Confirm link!", 
                    "This message is intended to confirm your registration on the website. \n" +
                    "If you have not registered, please ignore this message \n" + link);
                
                    _emailService.SendEmail(message);
         
               
                    if (!result.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden,
                        new Response { Status = "Error", Message = "User Failed to Create!" });
                    }


                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = "User create" });
    
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                new Response { Status = "Error", Message = "This role doesnot exist!" });
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password)) 
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles) 
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                var jwtToken = GetToken(authClaims);
                return Ok(new { 
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo,
                    role = userRoles[0]
                });
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims) 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                CurrentUser userDto = new CurrentUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Role = roles[0]
                }; 
                return Ok(userDto);
            }
            return NotFound();
        }
        [HttpGet("name/{username}")]
        public async Task<IActionResult> GetIfNameExist(string username) 
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user != null)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetIfidExist(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserDTOModel userDto = new UserDTOModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Role = null
                };
                return Ok(userDto);
            }
            return NotFound();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {

                var user = await _userManager.FindByEmailAsync(email);
         
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded) 
                {
                    return Redirect("https://errorguywithgun.github.io/MyBlazorAppSourse/ConfirmEmail") ;
                }
                return StatusCode(StatusCodes.Status403Forbidden,
                new Models.Response { Status = "Error", Message = "Can't Send" });

        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) 
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(nameof(ResetPasswordEmail), "Authentication", new {token, email = user.Email}, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Reset Password Link", "" + link);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, 
                    new Response { Status="Success", Message="Email sent to change password "});
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "User not exist" });
        }

        [HttpGet("ResetPasswordEmail")]
        public async Task<IActionResult> ResetPasswordEmail(string token, string email)
        {
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(email);
            return Redirect($"https://errorguywithgun.github.io/MyBlazorAppSourse/ResetPassword?token={encodedToken}&email={encodedEmail}");
         
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword) 
        {
            
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null) 
            {
                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (resetPasswordResult.Succeeded) 
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Password changed" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Faild", Message = "Password not changed" });

            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Faild", Message = "User not exist" });
        }


    }
}
