using CRM.Data.Entities;
using CRM.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    public class MachinesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiEndpoint = "/api/Machine";

        [BindProperty] public MachineModel MachineModel { get; set; }
        public List<MachineModel> Machines { get; set; }
        public List<ClientModel> Clients { get; set; }

        [TempData] public string SuccessMessage { get; set; }
        [TempData] public string ErrorMessage { get; set; }

        public MachinesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /* ---------- Page load ---------- */
        public async Task OnGetAsync()
        {
            MachineModel = new MachineModel();
            await LoadClientsAsync();
        }

        /* ---------- CREATE ---------- */
        public async Task<IActionResult> OnPost()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            AddJwt(client);

            var resp = await client.PostAsJsonAsync(ApiEndpoint, MachineModel);
            if (!resp.IsSuccessStatusCode)
            {
                ErrorMessage = $"Error creating machine. Status code: {resp.StatusCode}";
                return RedirectToPage();
            }
            SuccessMessage = "Machine created successfully.";
            return RedirectToPage();
        }

        /* ---------- UPDATE ---------- */
        public async Task<IActionResult> OnPostUpdate()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            AddJwt(client);

            var resp = await client.PutAsJsonAsync($"{ApiEndpoint}/{MachineModel.Id}", MachineModel);
            if (!resp.IsSuccessStatusCode)
            {
                ErrorMessage = $"Error updating machine. Status code: {resp.StatusCode}";
                return RedirectToPage();
            }
            SuccessMessage = "Machine updated successfully.";
            return RedirectToPage();
        }

        /* ---------- DELETE (AJAX) ---------- */
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            AddJwt(client);

            var resp = await client.PostAsync($"{ApiEndpoint}/{id}/delete", null);
            if (!resp.IsSuccessStatusCode)
            {
                return new JsonResult(new { success = false, message = "Delete failed." });
            }
            return new JsonResult(new { success = true, message = "Machine deleted." });
        }

        /* ---------- LIST (AJAX) ---------- */
        public async Task<JsonResult> OnPostList()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            AddJwt(client);

            var resp = await client.GetAsync(ApiEndpoint);
            var list = resp.IsSuccessStatusCode
                       ? JsonConvert.DeserializeObject<List<MachineModel>>(await resp.Content.ReadAsStringAsync())
                       : new List<MachineModel>();
            return new JsonResult(list);
        }

        /* ---------- Helpers ---------- */
        private async Task LoadClientsAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            AddJwt(client);

            var resp = await client.GetAsync("/api/Clients");
            Clients = resp.IsSuccessStatusCode
                      ? JsonConvert.DeserializeObject<List<ClientModel>>(await resp.Content.ReadAsStringAsync())
                      : new List<ClientModel>();
        }

        private void AddJwt(HttpClient client)
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    /* ---------- DTO ---------- */
    public class MachineModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string MachineIp { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }

        public ClientModel Client { get; set; }
    }
}
