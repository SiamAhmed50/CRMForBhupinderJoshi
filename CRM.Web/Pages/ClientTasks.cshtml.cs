using CRM.Data.Entities;
using CRM.Data.Enums;
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
    public class ClientTasksModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiEndpoint = "/api/ClientTask";

        [BindProperty]
        public ClientTaskModel ClientTaskModel { get; set; }
        public List<ClientTaskModel> ClientTasks { get; set; }
        public List<ClientModel> Clients { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public ClientTasksModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            ClientTaskModel = new ClientTaskModel();

            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            
                try
                {
                    AddAuthorizationToken(httpClient);

                    var clientResponse = await httpClient.GetAsync("/api/Clients");
                    var taskResponse = await httpClient.GetAsync(_apiEndpoint);

                    if (clientResponse.IsSuccessStatusCode)
                    {
                        var content = await clientResponse.Content.ReadAsStringAsync();
                        Clients = JsonConvert.DeserializeObject<List<ClientModel>>(content);
                    }
                    if (taskResponse.IsSuccessStatusCode)
                    {
                        var content = await taskResponse.Content.ReadAsStringAsync();
                        ClientTasks = JsonConvert.DeserializeObject<List<ClientTaskModel>>(content);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error loading data: {ex.Message}";
                    Clients = new List<ClientModel>();
                    ClientTasks = new List<ClientTaskModel>();
                }
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.PostAsJsonAsync(_apiEndpoint, ClientTaskModel);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error creating tasks. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Tasks have been created successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating tasks: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.PutAsJsonAsync($"{_apiEndpoint}/{ClientTaskModel.Id}", ClientTaskModel);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error updating tasks. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Tasks have been updated successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating tasks: {ex.Message}";
            }

            return RedirectToPage();
        }

        /*public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.DeleteAsync($"{_apiEndpoint}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error deleting tasks. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Tasks have been deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting tasks with ID {id}: {ex.Message}";
            }

            return RedirectToPage();
        }*/


        public async Task<IActionResult> OnPostDeleteAsync(int id)
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

                var url = $"{_apiEndpoint}/{id}/delete";
                var response = await httpClient.PostAsync(url, content: null);

                // Try to read and deserialize the JSON response
                var jsonString = await response.Content.ReadAsStringAsync();

                // Attempt to parse the response
                var result = JsonConvert.DeserializeObject<DeleteResponse>(jsonString);

                if (!response.IsSuccessStatusCode || result == null || result.success == false)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = result?.message ?? "Unknown error occurred while deleting task."
                    });
                }

                return new JsonResult(new
                {
                    success = true,
                    message = result.message ?? "Task deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = $"Error deleting task with ID {id}: {ex.Message}"
                });
            }
        }


        public async Task<IActionResult> OnPostListAsync()
        {
            try
            {
                await LoadTasksAsync();
                return new JsonResult(ClientTasks);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading tasks: {ex.Message}";
                return new JsonResult(new List<ClientTaskModel>());
            }
        }

        private async Task LoadTasksAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            
                try
                {
                    //AddAuthorizationToken(httpClient);

                    var response = await httpClient.GetAsync(_apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        ClientTasks = JsonConvert.DeserializeObject<List<ClientTaskModel>>(content);
                    }
                    else
                    {
                        ErrorMessage = $"Error loading tasks. Status code: {response.StatusCode}";
                        ClientTasks = new List<ClientTaskModel>();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error loading tasks: {ex.Message}";
                    ClientTasks = new List<ClientTaskModel>();
                }
            
        }

        public async Task<IActionResult> OnGetToggleTaskStatusAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                // Add the authorization token to the HTTP client
                AddAuthorizationToken(httpClient);

                // Call the API to toggle the task status
                var response = await httpClient.GetAsync($"{_apiEndpoint}/ToggleTaskStatus/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = $"Failed to toggle task status. Status code: {response.StatusCode}"
                    });
                }

                // Parse the response and return the updated status
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(content);

                return new JsonResult(new
                {
                    success = true,
                    status = result?.status
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        } 
        private void AddAuthorizationToken(HttpClient httpClient)
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

    public class ClientTaskModel
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public ClientTaskStatus Status { get; set; }
        public List<Tasks> Tasks { get; set; }
    }

    // Helper DTO to map the API response
    public class DeleteResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
