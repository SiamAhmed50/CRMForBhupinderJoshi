using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CRM.Web.Pages
{
    public class IndexModel : SecurePageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;
        private readonly string _clientsEndpoint = "/api/Clients";
        private readonly string _dashboardEndpoint = "/api/Dashboard";

        public IndexModel(
            IHttpClientFactory httpClientFactory,
            IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = apiSettings.Value.ApiUrl.TrimEnd('/');
        }

        // Dropdown & filters
        public List<ClientModel> Clients { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedClient { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        // API results
        public SummaryDto Summary { get; set; }
        public List<SeriesPoint> JobsSeries { get; set; }
        public List<SeriesPoint> TransactionsSeries { get; set; }

        // DTOs
        public class SummaryDto
        {
            // Jobs
            public int processed { get; set; }
            public int inProgress { get; set; }

            // Transactions
            public int totalTx { get; set; }
            public int successfulTx { get; set; }
            public int businessException { get; set; }
            public int systemException { get; set; }
        }


        public class SeriesPoint
        {
            public string date { get; set; }
            public int count { get; set; }
        }

        public class ClientModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        // Export DTOs
        public class JobDto
        {
            public int Id { get; set; }
            public int ClientId { get; set; }
            public int TasksId { get; set; }
            public string Status { get; set; }
            public DateTime Started { get; set; }
            public DateTime? Ended { get; set; }
        }
        public class JobTransactionDto
        {
            public int Id { get; set; }
            public int JobId { get; set; }
            public int Number { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string Comment { get; set; }
            public DateTime Timestamp { get; set; }
        }
        public async Task OnGetAsync()
        {
            // 1) Load clients
            await LoadClientsAsync();

            // 2) Default date range
            StartDate ??= DateTime.Today.AddDays(-30);
            EndDate ??= DateTime.Today;

            // 3) Prepare HttpClient with JWT
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // 4) Build query string
            var qp = new List<string>();
            if (SelectedClient.HasValue) qp.Add($"clientId={SelectedClient.Value}");
            if (StartDate.HasValue) qp.Add($"startDate={StartDate:yyyy-MM-dd}");
            if (EndDate.HasValue) qp.Add($"endDate={EndDate:yyyy-MM-dd}");
            var query = qp.Any() ? "?" + string.Join("&", qp) : string.Empty;

            // 5) Summary
            var summaryUrl = $"{_apiBaseUrl}{_dashboardEndpoint}/summary{query}";
            var summaryResp = await client.GetAsync(summaryUrl);
            if (summaryResp.IsSuccessStatusCode)
            {
                var summaryJson = await summaryResp.Content.ReadAsStringAsync();
                Summary = JsonConvert.DeserializeObject<SummaryDto>(summaryJson)
                          ?? new SummaryDto();
            }
            else
            {
                Summary = new SummaryDto();
            }

            // 6) Jobs Chart
            var jobsUrl = $"{_apiBaseUrl}{_dashboardEndpoint}/jobsChart{query}";
            var jobsResp = await client.GetAsync(jobsUrl);
            if (jobsResp.IsSuccessStatusCode)
            {
                var jobsJson = await jobsResp.Content.ReadAsStringAsync();
                JobsSeries = JsonConvert.DeserializeObject<List<SeriesPoint>>(jobsJson)
                                 ?? new List<SeriesPoint>();
            }
            else
            {
                JobsSeries = new List<SeriesPoint>();
            }

            // 7) Transactions Chart
            var txUrl = $"{_apiBaseUrl}{_dashboardEndpoint}/transactionsChart{query}";
            var txResp = await client.GetAsync(txUrl);
            if (txResp.IsSuccessStatusCode)
            {
                var txJson = await txResp.Content.ReadAsStringAsync();
                TransactionsSeries = JsonConvert.DeserializeObject<List<SeriesPoint>>(txJson)
                                         ?? new List<SeriesPoint>();
            }
            else
            {
                TransactionsSeries = new List<SeriesPoint>();
            }
        }


        private async Task LoadClientsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            var token = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.GetAsync(_apiBaseUrl + _clientsEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                Clients = new List<ClientModel>();
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var all = JsonConvert.DeserializeObject<List<Client>>(json) ?? new();
            Clients = all
                .Select(c => new ClientModel { Id = c.Id, Name = c.Name })
                .ToList();

            // insert “All”
            Clients.Insert(0, new ClientModel { Id = 0, Name = "Select Client" });
        }

        public async Task<IActionResult> OnGetExportJobsAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var qp = new List<string>();
            if (SelectedClient.HasValue) qp.Add($"clientId={SelectedClient.Value}");
            if (StartDate.HasValue) qp.Add($"startDate={StartDate:yyyy-MM-dd}");
            if (EndDate.HasValue) qp.Add($"endDate={EndDate:yyyy-MM-dd}");
            var query = qp.Any() ? "?" + string.Join("&", qp) : string.Empty;

            var jobsData = await client.GetFromJsonAsync<List<JobDto>>(
                $"{_apiBaseUrl}{_dashboardEndpoint}/jobsData{query}")
                ?? new List<JobDto>();

            var csv = new StringBuilder();
            csv.AppendLine("Id,ClientId,TasksId,Status,Started,Ended");
            foreach (var j in jobsData)
                csv.AppendLine($"{j.Id},{j.ClientId},{j.TasksId},{j.Status},{j.Started:O},{j.Ended:O}");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "jobs_export.csv");
        }

        public async Task<IActionResult> OnGetExportTransactionsAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var qp = new List<string>();
            if (SelectedClient.HasValue) qp.Add($"clientId={SelectedClient.Value}");
            if (StartDate.HasValue) qp.Add($"startDate={StartDate:yyyy-MM-dd}");
            if (EndDate.HasValue) qp.Add($"endDate={EndDate:yyyy-MM-dd}");
            var query = qp.Any() ? "?" + string.Join("&", qp) : string.Empty;

            var txData = await client.GetFromJsonAsync<List<JobTransactionDto>>(
                $"{_apiBaseUrl}{_dashboardEndpoint}/transactionsData{query}")
                ?? new List<JobTransactionDto>();

            var csv = new StringBuilder();
            csv.AppendLine("Id,JobId,Number,Description,Status,Comment,Timestamp");
            foreach (var t in txData)
                csv.AppendLine($"{t.Id},{t.JobId},{t.Number},\"{t.Description}\",{t.Status},\"{t.Comment}\",{t.Timestamp:O}");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "transactions_export.csv");
        }
    }
}
