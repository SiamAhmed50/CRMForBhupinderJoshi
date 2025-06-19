// ---------------------------
// UPDATED: AccountController.cs
// ---------------------------
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
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using CRM.API.ViewModels;
using CRM.Data.Models;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid model state" });

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { Message = "User registration failed", Errors = result.Errors });

            if (model.MenuIds?.Any() == true)
                await _unitOfWork.UserMenuRepository.AddUserMenusAsync(user.Id, model.MenuIds);

            if (model.ClientIds?.Any() == true)
                await _unitOfWork.UserClientRepository.AddUserClientsAsync(user.Id, model.ClientIds);

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

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
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

        [HttpGet("users")] 
        public async Task<IActionResult> GetAllUsers()
        {
            // 1. Load all non-admin users
            var users = _userManager.Users
                .Where(u => u.Email.ToLower() != "admin@crm.com")
                .ToList();

            var userList = new List<UserListDto>();

            foreach (var user in users)
            {
                // 2. Get menus for this user
                var menuEntities = await _unitOfWork.UserMenuRepository
                    .GetMenusByUserIdAsync(user.Id);
                var menuIds = menuEntities.Select(m => m.Id).ToList();

                // 3. Get clients for this user (you need a similar repo method)
                var clientEntities = await _unitOfWork.UserClientRepository
                    .GetClientsByUserIdAsync(user.Id);
                var clientIds = clientEntities.Select(c => c.Id).ToList();

                // 4. Add both sets of IDs into the DTO
                userList.Add(new UserListDto
                {
                    Name = user.UserName,
                    Email = user.Email,
                    MenuIds = menuIds,
                    ClientIds = clientIds
                });
            }

            return Ok(userList);
        }


        [HttpDelete("delete/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { Message = "Failed to delete user", Errors = result.Errors });

            return Ok(new { Message = "User deleted successfully" });
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
    }
}
