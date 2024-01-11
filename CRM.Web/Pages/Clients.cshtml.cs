using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly string apiEndpoint = "/api/Clients";

        [BindProperty]
        public ClientModel ClientModel { get; set; }
        public List<ClientModel> Clients { get; set; }

        public async Task OnGetAsync()
        {
            // Initialize ClientModel
            ClientModel = new ClientModel();
            // Generate unique ClientId
            ClientModel.ClientId = GenerateUniqueId();

            // Generate standard format LicenseNumber
            ClientModel.LicenseNumber = GenerateLicenseNumber();

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
            try
            {
                // Create a client using API call
                await CreateClientAsync(ClientModel);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating client: {ex.Message}");
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync(ClientModel client)
        {
            try
            {
                // Update a client using API call
                await UpdateClientAsync(client);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating client: {ex.Message}");
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                // Delete a client using API call
                await DeleteClientAsync(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting client with ID {id}: {ex.Message}");
                return RedirectToPage();
            }
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

       
        private async Task CreateClientAsync(ClientModel client)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PostAsJsonAsync(apiEndpoint, client);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error creating client. Status code: {response.StatusCode}");
                        throw new Exception($"Error creating client. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating client: {ex.Message}");
                    throw new Exception($"Error creating client: {ex.Message}");
                }
            }
        }

        private async Task UpdateClientAsync(ClientModel client)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PutAsJsonAsync($"{apiEndpoint}/{client.Id}", client);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error updating client. Status code: {response.StatusCode}");
                        throw new Exception($"Error updating client. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating client: {ex.Message}");
                    throw new Exception($"Error updating client: {ex.Message}");
                }
            }
        }

        private async Task DeleteClientAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.DeleteAsync($"{apiEndpoint}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error deleting client. Status code: {response.StatusCode}");
                        throw new Exception($"Error deleting client. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting client with ID {id}: {ex.Message}");
                    throw new Exception($"Error deleting client with ID {id}: {ex.Message}");
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
    }
}
