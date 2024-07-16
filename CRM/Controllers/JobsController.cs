using CRM.API.ViewModels;
using CRM.Data.Entities;
using CRM.Service.Helpers;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    includes: new Expression<Func<Job, object>>[]
                    {
                        ct => ct.Client,
                        ct => ct.Tasks
                    });

                var jobsViewModels = jobs.Select(j => new JobsViewModel
                {
                    Id = j.Id,
                    JobId = j.Id,
                    ClientId = j.Client.ClientId,
                    ClientName = j.Client.Name,
                    TaskId = j.Tasks.Id,
                    TaskName = j.Tasks.Name,
                    TaskStatus = j.Status.ToString(),
                    StartDate = j.Started.ToString(),
                    EndDate = j.Ended.ToString()
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
        public async Task<IActionResult> Create(Job job)
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
                var jobs = await _unitOfWork.JobRepository.GetAllAsync();
                while (jobs.Any(j => j.Id == newJobId))
                {
                    newJobId = GenerateUniqueId();
                }

                var clients = await _unitOfWork.ClientRepository.GetAllAsync();
                var client = clients.FirstOrDefault(f => f.ClientId == model.ClientId);

                if (client == null)
                {
                    return BadRequest("Invalid ClientId");
                }

                var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
                var task = tasks.FirstOrDefault(f => f.Id == model.TaskId);

                if (task == null)
                {
                    return BadRequest("Invalid TaskId");
                }

                var job = new Job
                {
                    Id = newJobId,
                    ClientId = client.Id,
                    TasksId = task.Id,
                    Status = model.Status,
                    Started = model.Started,
                    Ended = model.Ended
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
        public async Task<IActionResult> UpdateJob(int id, Job job)
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
