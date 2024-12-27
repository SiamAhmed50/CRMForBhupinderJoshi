using CRM.Data.Entities;
using CRM.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    public class ClientsModel : SecurePageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly string _apiBaseUrl;
        //private readonly string apiBaseUrl = "https://localhost:44300";
        private readonly string _apiEndpoint = "/api/Clients";

        [BindProperty]
        public ClientModel ClientModel { get; set; }
        public List<ClientModel> Clients { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public ClientsModel(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            //_apiBaseUrl = apiSettings.Value.ApiUrl;
        }

        public async Task OnGetAsync()
        {
            // Initialize ClientModel
            ClientModel = new ClientModel();
            // Generate unique ClientId
            ClientModel.ClientId = GenerateUniqueId();

            // Generate standard format LicenseNumber
            ClientModel.LicenseNumber = GenerateLicenseNumber();
            ClientModel.LicenseStartDate = DateTime.Today;

            // Set the end date as 30 days more from today's date
            ClientModel.LicenseEndDate = DateTime.Today.AddDays(30);
        }

        private int GenerateUniqueId()
        {
            Random random = new Random();
            return random.Next(1000, 1000000); // Adjust range as needed
        }

        private string GenerateLicenseNumber()
        {
            return $"LIC-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        public async Task<IActionResult> OnPost()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                var token = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await httpClient.PostAsJsonAsync(_apiEndpoint, ClientModel);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error creating client. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Client has been created successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating client: {ex.Message}");
                throw new Exception($"Error creating client: {ex.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                // Add Authorization token
                var token = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await httpClient.PutAsJsonAsync($"{_apiEndpoint}/{ClientModel.Id}", ClientModel);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error updating client. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }
                SuccessMessage = "Client has been updated successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating client: {ex.Message}");
                throw new Exception($"Error updating client: {ex.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                // Add Authorization token
                var token = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await httpClient.DeleteAsync($"{_apiEndpoint}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error deleting client. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }
                SuccessMessage = "Client has been deleted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting client with ID {id}: {ex.Message}");
                throw new Exception($"Error deleting client with ID {id}: {ex.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostList()
        {
            try
            {
                await LoadClientsAsync();
                return new JsonResult(Clients);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading clients: {ex.Message}");
                return new JsonResult(new List<ClientModel>());
            }
        }

        private async Task LoadClientsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                // Retrieve the token from the cookie
                var token = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    // Add the token to the Authorization header
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    Console.WriteLine("Token is missing or invalid.");
                    Clients = new List<ClientModel>();
                    return;
                }

                // Make the API call
                var response = await httpClient.GetAsync(_apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Clients = JsonConvert.DeserializeObject<List<ClientModel>>(content);
                }
                else
                {
                    Console.WriteLine($"Error loading clients. Status code: {response.StatusCode}");
                    Clients = new List<ClientModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading clients: {ex.Message}");
                Clients = new List<ClientModel>();
            }
        }

    }

    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseStartDate { get; set; }
        public DateTime LicenseEndDate { get; set; }
        public bool LicenseStatus { get; set; } = true;
    }
}
