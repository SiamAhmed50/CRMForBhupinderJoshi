using CRM.API.ViewModels;
using CRM.Data.Entities;
using CRM.Data.Enums;
using CRM.Service.Helpers;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Logs
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var logs = await _unitOfWork.JobLogsRepository.GetAllAsync(
                includes: new Expression<Func<JobLogs, object>>[]
                {
                    ct=>ct.Job,
                    ct => ct.Client,
                    ct => ct.Task,
                    ct => ct.Logs
                }
            );

                var logsViewModel = logs.Select(jobLog => new LogsViewModel
                {
                    Id = jobLog.Id,
                    ClientId = jobLog.Client.ClientId,
                    TaskId = jobLog.Task.Id,
                    TaskName = jobLog.Task.Name,
                    Logs = jobLog.Logs.Select(log => new LogViewModel
                    {
                        Id = log.Id,
                        Timestamp = log.Timestamp,
                        LogMessage = log.LogMessage,
                        LogType = Enum.GetName(typeof(LogType), log.LogType)
                    }).ToList()
                }).ToList();

                return Ok(logsViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading logs: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLog(int id)
        {
            var jobLog = await _unitOfWork.JobLogsRepository.GetAllAsync(filter: x=>x.Id==id, includes: i=>i.Logs);
            var logs = jobLog.FirstOrDefault();
            var log = logs.Logs;

            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // POST: api/Logs
        [HttpPost]
        public async Task<IActionResult> CreateJobLog(JobLogs log)
        {
            var createdLog = await _unitOfWork.JobLogsRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetLog", new { id = createdLog.Id }, createdLog);
        }

        // PUT: api/Logs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLog(int id, JobLogs log)
        {
            if (id != log.Id)
            {
                return BadRequest();
            }

            var updatedLog = await _unitOfWork.JobLogsRepository.UpdateAsync(log);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedLog);
        }

        // DELETE: api/Logs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var deleted = await _unitOfWork.LogsRepository.DeleteAsync(id.ToString());

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Logs/CreateLogs
        [HttpPost("CreateLogs")]
        public async Task<IActionResult> CreateLogs(CreateLog logModel)
        {
            try
            {
                var jobLogs = await _unitOfWork.JobLogsRepository.GetAllAsync(
                    
                    includes: new Expression<Func<JobLogs, object>>[]
                    {
                        ct => ct.Client,
                        ct => ct.Task,
                        ct => ct.Job
                    }
                );
                var existingJobLog = jobLogs.FirstOrDefault(f => f.JobId == logModel.JobId);

                var jobs = await _unitOfWork.JobRepository.GetAllAsync(
                    filter: j => j.Id == logModel.JobId,
                    includes: new Expression<Func<Job, object>>[]
                    {
                        j => j.Client,
                        j => j.Tasks
                    }
                );
                var job = jobs.FirstOrDefault();

                if (job == null)
                {
                    return BadRequest("Invalid Job Id.");
                }

                if (existingJobLog == null)
                {
                    var newJobLog = new JobLogs
                    {
                        JobId = logModel.JobId,
                        ClientId = job.ClientId,
                        TaskId = job.TasksId, 
                        Client = job.Client,
                        Job = job,
                        Task = job.Tasks,
                        Logs = new List<Logs>()
                    };

                    // Save the new JobLogs entity first to generate the Id
                    var addedJobLog = await _unitOfWork.JobLogsRepository.AddAsync(newJobLog);
                    await _unitOfWork.SaveChangesAsync();

                    // Create a new log and set the JoblogId
                    var log = CreateLogFromModel(logModel);
                    log.JoblogId = addedJobLog.Id; // Set the JoblogId
                    addedJobLog.Logs.Add(log);

                    // Update the JobLogs with the new Log
                    await _unitOfWork.JobLogsRepository.UpdateAsync(addedJobLog);
                }
                else
                {
                    existingJobLog.Logs.Add(CreateLogFromModel(logModel));
                    await _unitOfWork.JobLogsRepository.UpdateAsync(existingJobLog);
                }

                await _unitOfWork.SaveChangesAsync();
                return Ok("Logs created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // New GetLogsById method
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetLogsById(int jobId)
        {
            try
            {
                var logs = await _unitOfWork.JobLogsRepository.GetAllAsync(
                includes: new Expression<Func<JobLogs, object>>[]
                {
                    ct => ct.Job,
                    ct => ct.Client,
                    ct => ct.Task,
                    ct => ct.Logs
                }
                );

                if (logs == null || !logs.Any())
                {
                    return NotFound($"No logs found for JobId {jobId}");
                }

                var logsViewModel = logs.Where(w => w.JobId == jobId).Select(jobLog => new LogsViewModel
                {
                    Id = jobLog.Id,
                    ClientId = jobLog.Client.ClientId,
                    TaskId = jobLog.Task.Id,
                    TaskName = jobLog.Task.Name,
                    Logs = jobLog.Logs.Select(log => new LogViewModel
                    {
                        Id = log.Id,
                        Timestamp = log.Timestamp,
                        LogMessage = log.LogMessage,
                        LogType = Enum.GetName(typeof(LogType), log.LogType)
                    }).ToList()
                }).ToList();

                // Return empty list if no matching logs found, otherwise return the logs
                return Ok(logsViewModel.FirstOrDefault()?.Logs ?? new List<LogViewModel>());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading logs: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }


        private Logs CreateLogFromModel(CreateLog logModel)
        {
            return new Logs
            {
                Timestamp = logModel.Timestamp,
                LogMessage = logModel.LogMessage,
                LogType = logModel.LogType
            };
        }
    }
}
