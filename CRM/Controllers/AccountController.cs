using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.DbContext;
using CRM.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using CRM.Data.Entities;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using CRM.Service.Interfaces.UnitOfWork;
using CRM.API.ViewModels;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;


        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration,  IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;


        }

        [HttpPost("register")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid model state" });

            // Create new user
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { Message = "User registration failed", Errors = result.Errors });

            // Save MenuIds into UserMenus table using repo method
            if (model.MenuIds != null && model.MenuIds.Any())
            {
                await _unitOfWork.UserMenuRepository.AddUserMenusAsync(user.Id, model.MenuIds);
                await _unitOfWork.SaveChangesAsync();
            }

            return Ok(new { Message = "User registered successfully with menu access" });
        }




        [HttpPost("login")]
 
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid login request" });

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);

                // Set cookie (secure in production)
                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // set to true in production
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                Response.Cookies.Append("UserName", user.UserName);
                Response.Cookies.Append("UserEmail", user.Email);

                return Ok(new { Token = token, User = new { user.UserName, user.Email } });
            }

            return Unauthorized(new { Message = "Invalid username or password" });
        }


        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            HttpContext.Response.Cookies.Delete("UserName");
            HttpContext.Response.Cookies.Delete("UserEmail");
            HttpContext.Response.Cookies.Delete("UserRole");
            HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok(new { Message = "Logout successful" });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
{
new Claim(ClaimTypes.NameIdentifier, user.Id),
new Claim(ClaimTypes.Name, user.UserName)

};


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            // Get all users
            var users = _userManager.Users.ToList();

            

            var userList = new List<UserListDto>();

            foreach (var user in users)
            {
                var menuEntities = await _unitOfWork.UserMenuRepository.GetMenusByUserIdAsync(user.Id);
                var menuIds = menuEntities.Select(m => m.Id).ToList();

                userList.Add(new UserListDto
                {
                    Name = user.UserName,
                    Email = user.Email,
                    MenuIds = menuIds
                });
            }


            return Ok(userList);
        }

    }
}
