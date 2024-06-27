using CRM.API.ViewModels;
using CRM.Data.Entities;
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
        public async Task<IActionResult> GetLogs(int jobId)
        {
            try
            {
                var logs = await _unitOfWork.GenericJobLogsRepository.GetAllAsync(
                    jl => jl.ClientId == jobId,
                    include: ct => ct.Client,
                    ct => ct.Task,
                    ct => ct.Logs);

                var logsViewModel = logs.Select(jobLog => new LogsViewModel
                {
                    Id = jobLog.Id,
                    ClientId = jobLog.Client.ClientId,
                    TaskId = jobLog.Task.TaskId,
                    TaskName = jobLog.Task.Name,
                    Logs = jobLog.Logs.Select(log => new Logs
                    {
                        Id = log.Id,
                        Timestamp = log.Timestamp,
                        LogMessage = log.LogMessage,
                        LogLevel = log.LogLevel
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
            var log = await _unitOfWork.JobLogsRepository.GetByIdAsync(id);

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
            var deleted = await _unitOfWork.LogsRepository.DeleteAsync(id);

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
                var existingJobLog = _unitOfWork.JobLogsRepository.GetAllAsync(
                    include: ct => ct.Client,
                    ct => ct.Task,
                    ct => ct.Logs)
                    .Result
                    .FirstOrDefault(w => w.JobId == logModel.JobId);

                var job = _unitOfWork.JobRepository.GetAllAsync(
                    include: ct => ct.Client,
                    ct => ct.Tasks)
                    .Result
                    .FirstOrDefault(f => f.JobId == logModel.JobId);

                if (job == null)
                {
                    return BadRequest("Invalid Job Id.");
                }

                if (existingJobLog == null)
                {
                    var newJobLog = new JobLogs
                    {
                        JobId = logModel.JobId,
                        ClientId = job.Client.Id,
                        TaskId = job.Tasks.Id,
                        Logs = new List<Logs>()
                    };

                    newJobLog.Logs.Add(CreateLogFromModel(logModel));
                    await _unitOfWork.JobLogsRepository.AddAsync(newJobLog);
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

        private Logs CreateLogFromModel(CreateLog logModel)
        {
            return new Logs
            {
                Timestamp = logModel.Timestamp,
                LogMessage = logModel.LogMessage,
                LogLevel = logModel.LogLevel
            };
        }
    }
}
