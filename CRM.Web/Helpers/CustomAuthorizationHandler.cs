using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace CRM.UI.Helpers
{
    public class CustomAuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get the JWT token from the cookie
            var jwtToken = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwtToken))
            {
                // Add the token to the Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
