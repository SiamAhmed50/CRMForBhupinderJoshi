using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;

namespace CRM.UI.Helpers
{
    public class SecurePageModel : PageModel
    {
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var token = Request.Cookies["jwt"]; // Or from headers/local storage

            if (string.IsNullOrEmpty(token) || !ValidateToken(token))
            {
                context.Result = new RedirectToPageResult("/Identity/Account/Login");
            }

            base.OnPageHandlerExecuting(context);
        }

        private bool ValidateToken(string token)
        {
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