using CRM.Data.Entities;
using CRM.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

/*namespace CRM.Web.Pages
{
    public class ScheduleListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _scheduleApiEndpoint = "/api/Schedule";

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
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
*/
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
    public class ScheduleListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _scheduleApiEndpoint = "/api/Schedule";
       
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

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