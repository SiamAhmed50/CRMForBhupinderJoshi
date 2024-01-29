// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CRM.Data.DbContext;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Net;

namespace CRM.UI.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); 
            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();

                var apiBaseUrl = "https://localhost:44300"; // Replace this with the actual base URL of your API

                var loginRequest = new
                {
                    Email = Input.Email,
                    Password = Input.Password,
                    RememberMe = Input.RememberMe
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{apiBaseUrl}/api/Account/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Extract the JWT token from the response
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
                    var token = tokenResponse.Token;

                    // Set the token in the authentication cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Input.Email),
                        new Claim("access_token", token), // Add the token as a claim
                        // Add other claims if needed
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Set the token in the authentication cookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    });
                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    if (user != null && await _userManager.CheckPasswordAsync(user, Input.Password))
                    {

                        // Use SignInManager to sign in the user
                        await _signInManager.SignInAsync(user, new AuthenticationProperties
                        {
                            IsPersistent = Input.RememberMe,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        });

                        // Log information
                        _logger.LogInformation("User logged in.");

                        return LocalRedirect(returnUrl);
                    }
                } 
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                } 
                _logger.LogWarning($"User login failed with status code: {response.StatusCode}");
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again later.");
                return Page();
            } 
            return Page();
        } 
    }

    // Define a class to represent the authentication result from your API
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
    }

}

