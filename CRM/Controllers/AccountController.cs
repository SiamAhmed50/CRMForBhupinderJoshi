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

        [ApiExplorerSettings(IgnoreApi = true)]

       
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

            if (model.MenuIds != null && model.MenuIds.Any())
            {
                await _unitOfWork.UserMenuRepository.AddUserMenusAsync(user.Id, model.MenuIds);
            }

            return Ok(new { Message = "User registered successfully with menu access" });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Invalid login request" });
                }

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token, User = new { user.UserName, user.Email } });
                }

                return Unauthorized(new { Message = "Invalid username or password" });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            

                new Claim(ClaimTypes.Name, user.UserName)
                // Add additional claims (e.g., roles) if necessary
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
