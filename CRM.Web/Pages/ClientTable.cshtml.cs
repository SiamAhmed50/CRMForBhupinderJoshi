using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    public class ClientTableModel : PageModel
    {
        private readonly string apiBaseUrl = "https://localhost:44300";
        private readonly string apiEndpoint = "/api/Clients";

        public List<ClientModel> Clients { get; set; }

        public async Task OnGetAsync()
        {
            await LoadClientsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await DeleteClientAsync(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Error deleting client with ID {id}: {ex.Message}");
                return RedirectToPage(); 
            }
        }

        private async Task LoadClientsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new System.Uri(apiBaseUrl);
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

        private async Task DeleteClientAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new System.Uri(apiBaseUrl);
                    var response = await httpClient.DeleteAsync($"{apiEndpoint}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        // Log the unsuccessful response status code
                        Console.WriteLine($"Error deleting client with ID {id}. Status code: {response.StatusCode}");
                        // Optionally throw an exception or handle the error in another way
                        throw new Exception($"Error deleting client with ID {id}. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (consider using a logging framework like Serilog)
                    Console.WriteLine($"Error deleting client with ID {id}: {ex.Message}");
                    // Optionally throw an exception or handle the error in another way
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
