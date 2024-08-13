using CRM.Data.Entities;
using CRM.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    
    public class ClientsModel : PageModel
    {
        private readonly string apiBaseUrl = "https://localhost:44300";
        //private readonly string apiBaseUrl;
        private readonly string apiEndpoint = "/api/Clients";

        [BindProperty]
        public ClientModel ClientModel { get; set; }
        public List<ClientModel> Clients { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        //public ClientsModel(IOptions<ApiSettings> apiSettings)
        //{
        //    apiBaseUrl = apiSettings.Value.ApiUrl;
        //}
        public ClientsModel()
        {
            
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
            // Logic to generate a unique ClientId (4-6 digits)
            Random random = new Random();
            return random.Next(1000, 1000000); // Adjust range as needed
        }

        private string GenerateLicenseNumber()
        {
            // Logic to generate a standard format LicenseNumber
            // You can customize this logic based on your requirements
            return $"LIC-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
        public async Task<IActionResult> OnPost()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PostAsJsonAsync(apiEndpoint, ClientModel);

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
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PutAsJsonAsync($"{apiEndpoint}/{ClientModel.Id}", ClientModel);

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
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.DeleteAsync($"{apiEndpoint}/{id}");

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
            }

            return RedirectToPage();
        }
   
        public async Task<IActionResult> OnPostList()
        {
            try
            {
                // Get all clients as JSON for DataTable via AJAX
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
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.GetAsync(apiEndpoint);

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
