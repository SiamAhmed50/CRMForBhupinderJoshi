// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using CRM.Data.DbContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CRM.UI.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page(); // Show the logout page if accessed via GET
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            _logger.LogInformation("User logged out.");
            HttpContext.Response.Cookies.Delete("jwt");
            HttpContext.Response.Cookies.Delete("UserName");
            HttpContext.Response.Cookies.Delete("UserEmail");
            HttpContext.Response.Cookies.Delete("UserRole");
            return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToPage("/Account/Login");

        }
    }
}
