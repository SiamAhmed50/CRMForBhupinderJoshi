using System.IdentityModel.Tokens.Jwt;

namespace CRM.UI.Helpers
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the current request is for a Razor Page
            var path = context.Request.Path.ToString().ToLower();

            if (!path.Contains("/login") && !path.Contains("/api")) // Exclude login and API routes
            {
                var token = context.Request.Cookies["jwt"]; // Or fetch from headers/local storage
                if (string.IsNullOrEmpty(token) || !ValidateToken(token))
                {
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }
            }

            await _next(context);
        }

        private bool ValidateToken(string token)
        {
            // Basic token validation, e.g., checking expiration
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                return jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }

}
