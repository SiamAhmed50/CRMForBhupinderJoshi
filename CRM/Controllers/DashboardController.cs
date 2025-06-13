using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Data.Enums;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        } 
        [HttpGet("summary")] 
        public async Task<IActionResult> GetSummary(
    [FromQuery] int? clientId,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate)
        {
            // 1) Optionally verify client exists
            if (clientId.HasValue)
            {
                var exists = await _unitOfWork.ClientRepository
                                   .GetByIdAsync(c => c.Id == clientId.Value)
                                   != null;
                if (!exists)
                    return NotFound($"Client {clientId} not found.");
            }

            // 2) Fetch jobs, filtered by clientId and date range
            var jobs = (await _unitOfWork.JobRepository.GetAllAsync(filter: j =>
                (!clientId.HasValue || j.ClientId == clientId.Value) &&
                (!startDate.HasValue || j.Started >= startDate.Value) &&
                (!endDate.HasValue || j.Started <= endDate.Value)
            )).ToList();

            // 3) Compute in-progress vs processed
            var inProgress = jobs.Count(j => j.Status == JobTaskStatus.Running);
            var processed = jobs.Count - inProgress;

            // 4) Fetch transactions in date range, then intersect by JobId
            var txnsRaw = await _unitOfWork.JobTransactionsRepository.GetAllAsync(filter: t =>
                (!startDate.HasValue || t.Timestamp >= startDate.Value) &&
                (!endDate.HasValue || t.Timestamp <= endDate.Value)
            );
            var jobIds = jobs.Select(j => j.Id).ToHashSet();
            var txns = txnsRaw.Where(t => jobIds.Contains(t.JobId)).ToList();

            // 5) Compute transaction metrics
            var totalTx = txns.Count;
            var successfulTx = txns.Count(t =>
                t.Status.Equals("Success", StringComparison.OrdinalIgnoreCase));
            var businessExceptionTx = txns.Count(t =>
                t.Status.Equals("BusinessException", StringComparison.OrdinalIgnoreCase));
            var systemExceptionTx = totalTx
                - successfulTx
                - businessExceptionTx;

            // 6) Return only the requested metrics
            return Ok(new
            {
                processed,
                inProgress,
                totalTx,
                successfulTx,
                businessException = businessExceptionTx,
                systemException = systemExceptionTx
            });
        }

        // GET api/dashboard/jobsChart?clientId=&startDate=&endDate=
        [HttpGet("jobsChart")]
        public async Task<IActionResult> GetJobsChart(
            [FromQuery] int? clientId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            // 1) If clientId supplied, ensure it exists
            if (clientId.HasValue)
            {
                var exists = await _unitOfWork.ClientRepository
                    .GetByIdAsync(c => c.Id == clientId.Value)
                    != null;
                if (!exists)
                    return NotFound($"Client {clientId.Value} not found.");
            }

            // 2) Fetch jobs filtered by client & date
            var jobs = (await _unitOfWork.JobRepository.GetAllAsync(filter: j =>
                (!clientId.HasValue || j.ClientId == clientId.Value) &&
                (!startDate.HasValue || j.Started >= startDate.Value) &&
                (!endDate.HasValue || j.Started <= endDate.Value)
            )).ToList();

            // 3) Group by date and count
            var series = jobs
                .GroupBy(j => j.Started.Date)
                .Select(g => new {
                    date = g.Key.ToString("yyyy-MM-dd"),
                    count = g.Count()
                })
                .OrderBy(pt => pt.date)
                .ToList();

            return Ok(series);
        }

        // GET api/dashboard/transactionsChart?clientId=&startDate=&endDate=
        [HttpGet("transactionsChart")]
        public async Task<IActionResult> GetTransactionsChart(
            [FromQuery] int? clientId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            // 1) If clientId supplied, ensure it exists
            if (clientId.HasValue)
            {
                var exists = await _unitOfWork.ClientRepository
                    .GetByIdAsync(c => c.Id == clientId.Value)
                    != null;
                if (!exists)
                    return NotFound($"Client {clientId.Value} not found.");
            }

            // 2) Optionally collect job‐ids for this client
            HashSet<int> clientJobIds = null;
            if (clientId.HasValue)
            {
                clientJobIds = (await _unitOfWork.JobRepository.GetAllAsync(j => j.ClientId == clientId.Value))
                    .Select(j => j.Id)
                    .ToHashSet();
            }

            // 3) Fetch all transactions in the date window
            var allTx = (await _unitOfWork.JobTransactionsRepository.GetAllAsync()).ToList();
            var filtered = allTx.Where(t =>
                (!startDate.HasValue || t.Timestamp >= startDate.Value) &&
                (!endDate.HasValue || t.Timestamp <= endDate.Value) &&
                (clientJobIds == null || clientJobIds.Contains(t.JobId))
            );

            // 4) Group by date and count
            var series = filtered
                .GroupBy(t => t.Timestamp.Date)
                .Select(g => new {
                    date = g.Key.ToString("yyyy-MM-dd"),
                    count = g.Count()
                })
                .OrderBy(pt => pt.date)
                .ToList();

            return Ok(series);
        }


        // DTOs for full‐row export
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

        [HttpGet("jobsData")]
        public async Task<IActionResult> GetJobsData(
            [FromQuery] int? clientId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            // 1) filter jobs same as summary/chart
            var jobs = await _unitOfWork.JobRepository.GetAllAsync(filter: j =>
                (!clientId.HasValue || j.ClientId == clientId.Value) &&
                (!startDate.HasValue || j.Started >= startDate.Value) &&
                (!endDate.HasValue || j.Started <= endDate.Value)
            );

            // 2) project to DTO
            var list = jobs.Select(j => new JobDto
            {
                Id = j.Id,
                ClientId = j.ClientId,
                TasksId = j.TasksId,
                Status = j.Status.ToString(),
                Started = j.Started,
                Ended = j.Ended
            }).ToList();

            return Ok(list);
        }

        [HttpGet("transactionsData")]
        public async Task<IActionResult> GetTransactionsData(
            [FromQuery] int? clientId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            // 1) first fetch jobs to scope by client/date
            var jobIds = (await _unitOfWork.JobRepository.GetAllAsync(filter: j =>
                (!clientId.HasValue || j.ClientId == clientId.Value) &&
                (!startDate.HasValue || j.Started >= startDate.Value) &&
                (!endDate.HasValue || j.Started <= endDate.Value)
            )).Select(j => j.Id).ToHashSet();

            // 2) fetch transactions in date range
            var txns = await _unitOfWork.JobTransactionsRepository.GetAllAsync(filter: t =>
                jobIds.Contains(t.JobId) &&
                (!startDate.HasValue || t.Timestamp >= startDate.Value) &&
                (!endDate.HasValue || t.Timestamp <= endDate.Value)
            );

            // 3) project to DTO
            var list = txns.Select(t => new JobTransactionDto
            {
                Id = t.Id,
                JobId = t.JobId,
                Number = t.Number,
                Description = t.Description,
                Status = t.Status,
                Comment = t.Comment,
                Timestamp = t.Timestamp
            }).ToList();

            return Ok(list);
        }


    }
}
