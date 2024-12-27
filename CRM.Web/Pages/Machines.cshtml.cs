using CRM.Data.Entities;
using CRM.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{

    public class MachinesModel : PageModel
    {
        private readonly string apiBaseUrl = "https://localhost:44300";
        //private readonly string apiBaseUrl;
        private readonly string apiEndpoint = "/api/Machine";

        [BindProperty]
        public MachineModel MachineModel { get; set; }
        public List<MachineModel> Machines { get; set; }
        public List<ClientModel> Clients { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public MachinesModel(IOptions<ApiSettings> apiSettings)
        {
            apiBaseUrl = apiSettings.Value.ApiUrl;
        }
        //public MachinesModel()
        //{

        //}
        public async Task OnGetAsync()
        {

            MachineModel = new MachineModel();
            // Get the list of clients from your service or repository
            using (var httpClient = new HttpClient())
            {
                AddAuthorizationToken(httpClient);
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                var response = await httpClient.GetAsync("/api/Clients");
              

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Clients = JsonConvert.DeserializeObject<List<ClientModel>>(content);
                } 

            }

        }
        public async Task<IActionResult> OnPost()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    AddAuthorizationToken(httpClient);
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PostAsJsonAsync(apiEndpoint, MachineModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error creating Machine. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }

                    SuccessMessage = "Machine has been created successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating Machine: {ex.Message}");
                    throw new Exception($"Error creating Machine: {ex.Message}");
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
                    AddAuthorizationToken(httpClient);
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PutAsJsonAsync($"{apiEndpoint}/{MachineModel.Id}", MachineModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error updating Machine. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }
                    SuccessMessage = "Machine has been updated successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating Machine: {ex.Message}");
                    throw new Exception($"Error updating Machine: {ex.Message}");
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
                    AddAuthorizationToken(httpClient);
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.DeleteAsync($"{apiEndpoint}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error deleting Machine. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }
                    SuccessMessage = "Machine has been deleted successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting Machine with ID {id}: {ex.Message}");
                    throw new Exception($"Error deleting Machine with ID {id}: {ex.Message}");
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostList()
        {
            try
            {
                // Get all Machines as JSON for DataTable via AJAX
                await LoadMachinesAsync();
                return new JsonResult(Machines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading machines: {ex.Message}");
                return new JsonResult(new List<MachineModel>());
            }
        }

        private async Task LoadMachinesAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    AddAuthorizationToken(httpClient);
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Machines = JsonConvert.DeserializeObject<List<MachineModel>>(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading machines. Status code: {response.StatusCode}");
                        Machines = new List<MachineModel>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading clients: {ex.Message}");
                    Machines = new List<MachineModel>();
                }
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



    public class MachineModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string MachineIp { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Client Client { get; set; }
    }
}
