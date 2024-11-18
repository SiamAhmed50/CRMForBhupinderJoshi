using CRM.API.ViewModels;
using CRM.Data.Entities;
using CRM.Data.Enums;
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

        public JobsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
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
                    TaskStatus = Enum.GetName(typeof(JobTaskStatus), j.Status),
                    StartDate = j.Started.ToString(),
                    EndDate = j.Ended.ToString()
                }).ToList();

                return Ok(jobsViewModels);
            }
            catch (Exception ex)
            {
             
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
                
                return StatusCode(500, "Internal server error");
            }
        }

        private int GenerateUniqueId() => new Random().Next(1000, 1000000);

        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob(CRM.API.ViewModels.CreateJob model)
        {
            try
            {
                var clients = await _unitOfWork.ClientRepository.GetAllAsync();
                var client = clients.FirstOrDefault(f => f.ClientId == model.ClientId);

                if (client == null)
                {
                    return BadRequest("Invalid ClientId");
                }
                else if(client.LicenseStatus && client.LicenseEndDate > DateTime.UtcNow)
                {
                    var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
                    var task = tasks.FirstOrDefault(f => f.TaskId == model.TaskId);

                    if (task == null)
                    {
                        return BadRequest("Invalid TaskId");
                    } 
                    var job = new Job
                    {
                       
                        ClientId = client.ClientId,
                        TasksId = task.TaskId,
                        Status = JobTaskStatus.Running,
                        Started = DateTime.UtcNow,
                        Client = client,
                        Tasks = task

                    };

                    var createdJob = await _unitOfWork.JobRepository.AddAsync(job);
                    await _unitOfWork.SaveChangesAsync();

                    return CreatedAtAction("GetJob", new { id = createdJob.Id }, createdJob);
                }
                else
                {
                    return BadRequest("Client License expired!");
                }
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("UpdateJob")]
        public async Task<IActionResult> UpdateJob(CRM.API.ViewModels.UpdateJob model)
        {
          
            try
            {
                var jobs = await _unitOfWork.JobRepository.GetAllAsync(
                includes: new Expression<Func<Job, object>>[]
                    {
                        ct => ct.Client,
                        ct => ct.Tasks
                    }); 
                var job = jobs.FirstOrDefault(f => f.Id == model.JobId);
                if (job == null)
                {
                    return BadRequest("Invalid JobId");
                }
                else
                {
                    var clients = await _unitOfWork.ClientRepository.GetAllAsync();
                    var client = clients.FirstOrDefault(f => f.ClientId == job.Client.ClientId);

                    if (client == null)
                    {
                        return BadRequest("Invalid ClientId");
                    }
                    else if (client.LicenseStatus && client.LicenseEndDate > DateTime.UtcNow)
                    {
                        var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
                        var task = tasks.FirstOrDefault(f => f.TaskId == job.Tasks.TaskId);

                        if (task == null)
                        {
                            return BadRequest("Invalid TaskId");
                        } 
                        job.Ended = model.Ended;
                        job.Status = model.Status;


                        var updatedJob = await _unitOfWork.JobRepository.UpdateAsync(job);
                        await _unitOfWork.SaveChangesAsync();

                        return CreatedAtAction("UpdateJob", new { id = updatedJob.Id }, updatedJob);
                    }
                    else
                    {
                        return BadRequest("Client License expired!");
                    }
                }

                
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                // Fetch the job first to perform custom checks
                var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
                if (job == null)
                {
                    return NotFound("Job not found");
                } 
                // Perform deletion if checks pass
                var deleted = await _unitOfWork.JobRepository.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound("Failed to delete the job");
                }

                await _unitOfWork.SaveChangesAsync();
                return NoContent(); // Successfully deleted
            }
            catch (Exception ex)
            {
                // Log the exception if needed (omitted here for brevity)
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
             
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteJob(int id)
        //{
        //    try
        //    {
        //        var deleted = await _unitOfWork.JobRepository.DeleteAsync(id);

        //        if (!deleted)
        //        {
        //            return NotFound();
        //        }

        //        await _unitOfWork.SaveChangesAsync();
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
              
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}
