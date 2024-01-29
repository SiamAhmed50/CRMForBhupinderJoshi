using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using CRM.API.ViewModels;
using CRM.Service.Helpers;
using System.Linq.Expressions;

namespace CRM.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WeatherForecastController> _logger;

    
        public JobsController(IUnitOfWork unitOfWork, ILogger<WeatherForecastController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            
            var Jobs = await _unitOfWork.JobRepository.GetAllAsync(includeProperties: new Expression<Func<Jobs, object>>[]
             {
                ct => ct.Client,
                     ct => ct.Tasks
             });
            var data = new List<JobsViewModel>();
            foreach (var job in Jobs)
            {
                var model = new JobsViewModel();
                model.Id = job.Id;
                model.ClientId = job.Client.ClientId;
                model.ClientName = job.Client.Name;
                model.TaskId = job.Tasks.TaskId;
                model.TaskName = job.Tasks.Name;
                model.TaskStatus = job.TaskStatus.ToString();
                model.StartDate = job.StartDate.ToString();
                model.EndDate = job.EndDate.ToString(); 
                data.Add(model);
            }

            return Ok(data);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var Job = await _unitOfWork.JobRepository.GetByIdAsync(id);

            if (Job == null)
            {
                return NotFound();
            }

            return Ok(Job);
        }

        // POST: api/Jobs
        [HttpPost]
        public async Task<IActionResult> Create(Jobs Job)
        {

            var createdJob = await _unitOfWork.JobRepository.AddAsync(Job);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = createdJob.Id }, createdJob);
        }

        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob(CreateJob model)
        {
            var job = new Jobs();
            job.ClientId = _unitOfWork.ClientRepository.GetAllAsync().Result.FirstOrDefault(w => w.ClientId == model.ClientId).Id;
            job.TaskId = _unitOfWork.TaskRepository.GetAllAsync().Result.FirstOrDefault(w => w.TaskId == model.TaskId).Id;
            job.TaskStatus = model.Status;
            job.StartDate = model.Started; ;
            job.EndDate = model.Ended; 
            var createdJob = await _unitOfWork.JobRepository.AddAsync(job);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = createdJob.Id }, createdJob);
        }

        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, Jobs Job)
        {
            if (id != Job.Id)
            {
                return BadRequest();
            }

            var updatedJob = await _unitOfWork.JobRepository.UpdateAsync(Job);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedJob);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var deleted = await _unitOfWork.JobRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }



    }
}
