using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using CRM.Service.Helpers;
using CRM.API.ViewModels;
using CRM.Data.Models;

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
            var jobLogs = await _unitOfWork.GenericJobLogsRepository
                 .GetAllAsync(includeProperties: new Expression<Func<JobLogs, object>>[]
             {
                ct => ct.Client,
                ct => ct.Task,
                ct => ct.Logs
             }); 

            var data = new LogsViewModel();
            foreach (var jobLog in jobLogs)
            {
                var model = new LogsViewModel();
                var logList = new List<Logs>();

                model.Id = jobLog.Id;
                model.ClientId = jobLog.Client.ClientId;
                model.TaskId = jobLog.Task.TaskId;
                model.TaskName = jobLog.Task.Name; 
                foreach(var log in jobLog.Logs)
                {
                    var logModel = new Logs();
                    logModel.Id = log.Id;
                    logModel.Timestamp = log.Timestamp;
                    logModel.LogMessage = log.LogMessage;
                    logModel.LogLevel = log.LogLevel;
                    logList.Add(logModel);
                }
                model.logs = logList;
                data = model;
            }
            return Ok(data.logs);
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLog(int id)
        {
            var Log = await _unitOfWork.JobLogsRepository.GetByIdAsync(id);

            if (Log == null)
            {
                return NotFound();
            }

            return Ok(Log);
        }

        // POST: api/Logs
        [HttpPost]
        public async Task<IActionResult> CreateJobLog(JobLogs Log)
        {
            
            var createdLog = await _unitOfWork.JobLogsRepository.AddAsync(Log);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetLog", new { id = createdLog.Id }, createdLog);
        }

        // PUT: api/Logs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLog(int id, JobLogs Log)
        {
            if (id != Log.Id)
            {
                return BadRequest();
            }

            var updatedLog = await _unitOfWork.JobLogsRepository.UpdateAsync(Log);
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



        [HttpPost("CreateLogs")]
        public async Task<IActionResult> CreateLogs(CreateLog logModel)
        {
            try
            {
                // Check if there is an existing JobLogs for the given ClientId and TaskId
                var existingJobLogs = _unitOfWork.JobLogsRepository
                    .GetAllAsync(includeProperties: new Expression<Func<JobLogs, object>>[]
             {
                ct => ct.Client,
                ct => ct.Task,
                ct => ct.Logs
             }).Result.Where(w => w.Client.ClientId == logModel.ClientId && w.Task.TaskId == logModel.TaskId).FirstOrDefault();

                if (existingJobLogs == null)
                {
                    var clientId = _unitOfWork.ClientRepository.GetAllAsync().Result.FirstOrDefault(w => w.ClientId == logModel.ClientId).Id;
                    var taskId = _unitOfWork.TaskRepository.GetAllAsync().Result.FirstOrDefault(w => w.TaskId == logModel.TaskId).Id;
                    // Create new JobLogs and Logs
                    var newJobLogs = new JobLogs
                    {
                        ClientId = clientId,
                        TaskId = taskId,
                        Logs = new List<Logs>()
                    };

                    var newLog = CreateLogFromModel(logModel);
                    newJobLogs.Logs.Add(newLog);

                    await _unitOfWork.JobLogsRepository.AddAsync(newJobLogs);
                }
                else
                {
                    // Update existing JobLogs by adding new Logs
                    var newLog = CreateLogFromModel(logModel);
                    newLog.JobLogs = existingJobLogs;
                    existingJobLogs.Logs.Add(newLog);
                    await _unitOfWork.JobLogsRepository.UpdateAsync(existingJobLogs);
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok("Logs created successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
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
