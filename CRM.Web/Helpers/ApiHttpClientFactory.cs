using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Helpers
{
    public class ApiHttpClientFactory
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiHttpClientFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient CreateClient(string apiBaseUrl)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

            // Retrieve the access token from the authentication cookie
            var accessToken = GetAccessToken();

            // Set the access token in the Authorization header
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return httpClient;
        }

        private string GetAccessToken()
        {
            var result = _httpContextAccessor.HttpContext?.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            return result?.Principal?.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
        }
    }
}
