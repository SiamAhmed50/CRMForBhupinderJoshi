using CRM.Data.Entities;
using CRM.Service.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    
    public class ClientJobsModel : PageModel
    {
        private readonly string apiBaseUrl = "https://localhost:44300";
        private readonly string apiEndpoint = "/api/Jobs";
        private readonly ApiHttpClientFactory _apiHttpClientFactory;

        public ClientJobsModel(ApiHttpClientFactory apiHttpClientFactory)
        {
            _apiHttpClientFactory = apiHttpClientFactory;
        }
        [BindProperty]
        public List<JobsViewModel> JobViewModel { get; set; }
        public LogsViewModel LogViewModel { get; set; }
        public List<Logs> Logs { get; set; }
       
        public async Task OnGetAsync()
        {
           
          
        }


        public async Task<IActionResult> OnPostList()
        {
            try
            {
                // Get all clients as JSON for DataTable via AJAX
                await LoadJobsAsync();
                return new JsonResult(JobViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Jobs: {ex.Message}");
                return new JsonResult(new List<JobsViewModel>());
            }
        }

        private async Task LoadJobsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);

                    // Retrieve the access token from the authentication cookie
                    var accessToken = await GetAccessTokenAsync();

                    // Set the access token in the Authorization header
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }

                    var response = await httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JobViewModel = JsonConvert.DeserializeObject<List<JobsViewModel>>(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading jobs. Status code: {response.StatusCode}");
                        JobViewModel = new List<JobsViewModel>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading jobs: {ex.Message}");
                    JobViewModel = new List<JobsViewModel>();
                }
            }
        }


        public async Task<IActionResult> OnPostLogList()
        {
            try
            {
                // Get all clients as JSON for DataTable via AJAX
                await LoadLogsAsync();
                return new JsonResult(Logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Jobs: {ex.Message}");
                return new JsonResult(new List<LogsViewModel>());
            }
        }

        private async Task LoadLogsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);

                    // Retrieve the access token from the authentication cookie
                    var accessToken = await GetAccessTokenAsync();

                    // Set the access token in the Authorization header
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }

                    var response = await httpClient.GetAsync("/api/Logs");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Logs = JsonConvert.DeserializeObject<List<Logs>>(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading jobs. Status code: {response.StatusCode}");
                        Logs = new List<Logs>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading jobs: {ex.Message}");
                    Logs = new List<Logs>();
                }
            }
        }


        private async Task<string> GetAccessTokenAsync()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return result?.Principal?.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
        }

    }


     
}
