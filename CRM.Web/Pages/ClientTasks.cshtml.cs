using CRM.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    
    public class ClientTasksModel : PageModel
    {
        private readonly string apiBaseUrl = "https://localhost:44300";
        private readonly string apiEndpoint = "/api/ClientTask";

        [BindProperty]
        public ClientTaskModel ClientTaskModel { get; set; }
        public List<ClientTaskModel> ClientTasks { get; set; }
        public List<ClientModel> Clients { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public async Task OnGetAsync()
        {
           
            ClientTaskModel = new ClientTaskModel(); 
            // Get the list of clients from your service or repository
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                var response = await httpClient.GetAsync("/api/Clients");
                var response1 = await httpClient.GetAsync("/api/ClientTask");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Clients = JsonConvert.DeserializeObject<List<ClientModel>>(content);
                }
                if (response1.IsSuccessStatusCode)
                {
                    var content = await response1.Content.ReadAsStringAsync();
                    ClientTasks = JsonConvert.DeserializeObject<List<ClientTaskModel>>(content);
                }

            }
             
        } 
        public async Task<IActionResult> OnPost()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PostAsJsonAsync(apiEndpoint, ClientTaskModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error creating Tasks. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }

                    SuccessMessage = "Tasks has been created successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating tasks: {ex.Message}");
                    throw new Exception($"Error creating tasks: {ex.Message}");
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
                    var response = await httpClient.PutAsJsonAsync($"{apiEndpoint}/{ClientTaskModel.Id}", ClientTaskModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error updating tasks. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }
                    SuccessMessage = "Tasks has been updated successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating tasks: {ex.Message}");
                    throw new Exception($"Error updating tasks: {ex.Message}");
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
                        ErrorMessage = $"Error deleting tasks. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }
                    SuccessMessage = "Tasks has been deleted successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting tasks with ID {id}: {ex.Message}");
                    throw new Exception($"Error deleting tasks with ID {id}: {ex.Message}");
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
                var res =  new JsonResult(ClientTasks);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading clients: {ex.Message}");
                return new JsonResult(new List<ClientTaskModel>());
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
                        ClientTasks = JsonConvert.DeserializeObject<List<ClientTaskModel>>(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading clients. Status code: {response.StatusCode}");
                        ClientTasks = new List<ClientTaskModel>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading clients: {ex.Message}");
                    ClientTasks = new List<ClientTaskModel>();
                }
            }
        }
    }

    public class ClientTaskModel
    {
        public int Id { get; set; }
        public Client Client { get; set; }

        // Foreign key for Client
        public int ClientId { get; set; }

        // Navigation property for Tasks
        public List<Tasks> Tasks { get; set; }
    }
}
