using CRM.Data.Entities;
using CRM.Service.Helpers;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IUnitOfWork unitOfWork, ILogger<JobsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                var jobs = await _unitOfWork.JobRepository.GetAllAsync(
                    includeProperties: new Expression<Func<Jobs, object>>[]
                    {
                        ct => ct.Client,
                        ct => ct.Tasks
                    });

                var jobsViewModels = jobs.Select(j => new JobsViewModel
                {
                    Id = j.Id,
                    JobId = j.JobId,
                    ClientId = j.Client.ClientId,
                    ClientName = j.Client.Name,
                    TaskId = j.Tasks.TaskId,
                    TaskName = j.Tasks.Name,
                    TaskStatus = j.TaskStatus.ToString(),
                    StartDate = j.StartDate.ToString(),
                    EndDate = j.EndDate.ToString()
                }).ToList();

                return Ok(jobsViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving jobs");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            try
            {
                var job = await _unitOfWork.JobRepository.GetByIdAsync(id);

                if (job == null)
                {
                    return NotFound();
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving job");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Jobs job)
        {
            try
            {
                var createdJob = await _unitOfWork.JobRepository.AddAsync(job);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtAction("GetJob", new { id = createdJob.Id }, createdJob);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job");
                return StatusCode(500, "Internal server error");
            }
        }

        private int GenerateUniqueId() => new Random().Next(1000, 1000000);

        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob(CRM.API.ViewModels.CreateJob model)
        {
            try
            {
                var newJobId = GenerateUniqueId();
                while (await _unitOfWork.JobRepository.GetAllAsync()
                    .AnyAsync(j => j.JobId == newJobId))
                {
                    newJobId = GenerateUniqueId();
                }

                var client = await _unitOfWork.ClientRepository.GetAllAsync()
                    .FirstOrDefaultAsync(f => f.ClientId == model.ClientId);

                if (client == null)
                {
                    return BadRequest("Invalid ClientId");
                }

                var task = await _unitOfWork.TaskRepository.GetAllAsync()
                    .FirstOrDefaultAsync(f => f.TaskId == model.TaskId);

                if (task == null)
                {
                    return BadRequest("Invalid TaskId");
                }

                var job = new Jobs
                {
                    JobId = newJobId,
                    ClientId = client.Id,
                    TaskId = task.Id,
                    TaskStatus = model.Status,
                    StartDate = model.Started,
                    EndDate = model.Ended
                };

                var createdJob = await _unitOfWork.JobRepository.AddAsync(job);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtAction("GetJob", new { id = createdJob.Id }, createdJob);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, Jobs job)
        {
            try
            {
                if (id != job.Id)
                {
                    return BadRequest();
                }

                var updatedJob = await _unitOfWork.JobRepository.UpdateAsync(job);
                await _unitOfWork.SaveChangesAsync();

                return Ok(updatedJob);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                var deleted = await _unitOfWork.JobRepository.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting job");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
