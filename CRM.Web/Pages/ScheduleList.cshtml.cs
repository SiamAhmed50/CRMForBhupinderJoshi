using CRM.Data.Entities;
using CRM.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Web.Pages
{
    public class ScheduleListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _scheduleApiEndpoint = "/api/Schedule";

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public Schedule ScheduleModel { get; set; }

        public List<Schedule> ScheduleList { get; set; }

        public ScheduleListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> OnPostList()
        {
            try
            {
                await LoadSchedulesAsync();
                return new JsonResult(ScheduleList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading schedules: {ex.Message}");
                return new JsonResult(new List<Schedule>());
            }
        }

        private async Task LoadSchedulesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);
                var response = await httpClient.GetAsync(_scheduleApiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    ScheduleList = JsonConvert.DeserializeObject<List<Schedule>>(content);
                }
                else
                {
                    Console.WriteLine($"Error loading schedules. Status code: {response.StatusCode}");
                    ScheduleList = new List<Schedule>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading schedules: {ex.Message}");
                ScheduleList = new List<Schedule>();
            }
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

                var response = await httpClient.PutAsJsonAsync($"{_scheduleApiEndpoint}/{ScheduleModel.Id}", ScheduleModel);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error updating schedule. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }
                SuccessMessage = "Schedule has been updated successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating schedule: {ex.Message}");
                throw new Exception($"Error updating schedule: {ex.Message}");
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

                var response = await httpClient.DeleteAsync($"{_scheduleApiEndpoint}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error deleting schedule. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }
                SuccessMessage = "Schedule has been deleted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting schedule with ID {id}: {ex.Message}");
                throw new Exception($"Error deleting schedule with ID {id}: {ex.Message}");
            }

            return RedirectToPage();
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

