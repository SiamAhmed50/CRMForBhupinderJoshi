using CRM.Data.Entities;
using CRM.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRM.Web.Pages
{
    public class ScheduleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _scheduleApiEndpoint = "/api/Schedule";
        private readonly string _clientApiEndpoint = "/api/Clients";
        private readonly string _taskApiEndpoint = "/api/ClientTask";

        [BindProperty]
        public Schedule Schedule { get; set; }
        public List<Client> Clients { get; set; }
        public List<ClientTask> ClientTasks { get; set; }
        public List<string> TimeZones { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public List<Schedule> ScheduleList { get; set; }

        public ScheduleModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            Schedule = new Schedule();
            TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.DisplayName).ToList();

            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var clientResponse = await httpClient.GetAsync(_clientApiEndpoint);
                var taskResponse = await httpClient.GetAsync(_taskApiEndpoint);

                if (clientResponse.IsSuccessStatusCode)
                {
                    var clientContent = await clientResponse.Content.ReadAsStringAsync();
                    Clients = JsonConvert.DeserializeObject<List<Client>>(clientContent);
                }

                if (taskResponse.IsSuccessStatusCode)
                {
                    var taskContent = await taskResponse.Content.ReadAsStringAsync();
                    ClientTasks = JsonConvert.DeserializeObject<List<ClientTask>>(taskContent);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading data: {ex.Message}";
                Clients = new List<Client>();
                ClientTasks = new List<ClientTask>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.PostAsJsonAsync(_scheduleApiEndpoint, Schedule);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error creating schedule. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Schedule created successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating schedule: {ex.Message}";
            }

            return RedirectToPage("/ScheduleList");
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
}
