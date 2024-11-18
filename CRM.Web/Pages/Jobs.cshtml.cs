using CRM.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using CRM.UI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using CRM.API.ViewModels;

namespace CRM.UI.Pages
{
    //[Authorize]
    public class JobsModel : PageModel
    {
        //private readonly string apiBaseUrl = "https://localhost:44300";
        private readonly string apiBaseUrl;
        private readonly string apiEndpoint = "/api/Jobs";

        [BindProperty]
        public List<JobsViewModel> JobLogsList { get; set; }
        public List<LogViewModel> LogsList { get; set; }

        [BindProperty]
        public JobLogs JobLog { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


        public JobsModel(IOptions<ApiSettings> apiSettings)
        {
            apiBaseUrl = apiSettings.Value.ApiUrl;
        }


        //public JobsModel()
        //{

        //}

        public async Task OnGetAsync()
        {

        }

        private int GenerateUniqueId()
        {
            Random random = new Random();
            return random.Next(1000, 1000000);
        }

        private string GenerateLicenseNumber()
        {
            return $"LIC-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.PostAsJsonAsync(apiEndpoint, JobLog);

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error creating job log. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }

                    SuccessMessage = "Job log has been created successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating job log: {ex.Message}");
                    ErrorMessage = $"Error creating job log: {ex.Message}";
                    return RedirectToPage();
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.DeleteAsync($"{apiEndpoint}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Error deleting job log. Status code: {response.StatusCode}";
                        return RedirectToPage();
                    }
                    SuccessMessage = "Job log has been deleted successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting job log with ID {id}: {ex.Message}");
                    ErrorMessage = $"Error deleting job log with ID {id}: {ex.Message}";
                    return RedirectToPage();
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostList()
        {
            try
            {
                await LoadJobsAsync();
                var res = new JsonResult(JobLogsList);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading job logs: {ex.Message}");
                return new JsonResult(new List<JobLogs>());
            }
        }

        private async Task LoadJobsAsync()
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
                        JobLogsList = JsonConvert.DeserializeObject<List<JobsViewModel>>(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading job logs. Status code: {response.StatusCode}");
                        JobLogsList = new List<JobsViewModel>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading job logs: {ex.Message}");
                    JobLogsList = new List<JobsViewModel>();
                }
            }
        }
        public async Task<IActionResult> OnPostLogList(int id)
        {
            try
            {
                await LoadLogsAsync(id);
                var res = new JsonResult(LogsList);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading job logs: {ex.Message}");
                return new JsonResult(new List<JobLogs>());
            }
        }
        private async Task LoadLogsAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri(apiBaseUrl);
                    var response = await httpClient.GetAsync($"/api/Logs/Job/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        LogsList = JsonConvert.DeserializeObject<List<LogViewModel>>(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading job logs. Status code: {response.StatusCode}");
                        LogsList = new List<LogViewModel>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading job logs: {ex.Message}");
                    LogsList = new List<LogViewModel>();
                }
            }
        }
    }
}
