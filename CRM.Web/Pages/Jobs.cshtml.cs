using CRM.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using CRM.API.ViewModels;
using Microsoft.Extensions.Options;

namespace CRM.Web.Pages
{
    public class JobsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiEndpoint = "/api/Jobs";
        private readonly string _apitransactionsEndpoint = "/api/Transactions/Job";

        [BindProperty]
        public List<JobsViewModel> JobLogsList { get; set; }

        public List<LogViewModel> LogsList { get; set; }

        public List<JobTransactionsViewModel> TransactionsList { get; set; }

        [BindProperty]
        public JobLogs JobLog { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public JobsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.PostAsJsonAsync(_apiEndpoint, JobLog);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error creating job log. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Job log has been created successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating job log: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.DeleteAsync($"{_apiEndpoint}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error deleting job log. Status code: {response.StatusCode}";
                    return RedirectToPage();
                }

                SuccessMessage = "Job log has been deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting job log with ID {id}: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostList()
        {
            try
            {
                await LoadJobsAsync();
                return new JsonResult(JobLogsList);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading job logs: {ex.Message}";
                return new JsonResult(new List<JobLogs>());
            }
        }

        private async Task LoadJobsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.GetAsync(_apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JobLogsList = JsonConvert.DeserializeObject<List<JobsViewModel>>(content);
                }
                else
                {
                    ErrorMessage = $"Error loading job logs. Status code: {response.StatusCode}";
                    JobLogsList = new List<JobsViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading job logs: {ex.Message}";
                JobLogsList = new List<JobsViewModel>();
            }
        }

        public async Task<IActionResult> OnPostLogList(int id)
        {
            try
            {
                await LoadLogsAsync(id);
                return new JsonResult(LogsList);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading logs for job ID {id}: {ex.Message}";
                return new JsonResult(new List<LogViewModel>());
            }
        }

        private async Task LoadLogsAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.GetAsync($"/api/Logs/Job/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LogsList = JsonConvert.DeserializeObject<List<LogViewModel>>(content);
                }
                else
                {
                    ErrorMessage = $"Error loading logs for job ID {id}. Status code: {response.StatusCode}";
                    LogsList = new List<LogViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading logs for job ID {id}: {ex.Message}";
                LogsList = new List<LogViewModel>();
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

        // Method to fetch transactions for a specific job
        public async Task<IActionResult> OnPostTransactionList(int id)
        {
            try
            {
                await LoadTransactionsAsync(id);
                return new JsonResult(TransactionsList);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading transactions for job ID {id}: {ex.Message}";
                return new JsonResult(new List<JobTransactionsViewModel>());
            }
        }

        private async Task LoadTransactionsAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                AddAuthorizationToken(httpClient);

                var response = await httpClient.GetAsync($"/api/Transactions/Job/{id}");
                //var response = await httpClient.GetAsync($"{_apitransactionsEndpoint}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    TransactionsList = JsonConvert.DeserializeObject<List<JobTransactionsViewModel>>(content);
                }
                else
                {
                    ErrorMessage = $"Error loading transactions for job ID {id}. Status code: {response.StatusCode}";
                    TransactionsList = new List<JobTransactionsViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading transactions for job ID {id}: {ex.Message}";
                TransactionsList = new List<JobTransactionsViewModel>();
            }
        }

    }
}
