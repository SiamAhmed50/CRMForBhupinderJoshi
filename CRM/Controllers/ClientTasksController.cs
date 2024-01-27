using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using System.Linq.Expressions;
using CRM.API.ViewModels;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientTasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientTasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientTask>>> GetClientTasks()
        {
            var clientTasks = await _unitOfWork.ClientTaskRepository
             .GetAllAsync(includeProperties: new Expression<Func<ClientTask, object>>[]
             {
                ct => ct.Client,
                ct => ct.Tasks
             });
            return Ok(clientTasks.ToList());
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientTask(int id)
        {
            var clientTask = await _unitOfWork.ClientTaskRepository.GetByIdAsync(id);

            if (clientTask == null)
            {
                return NotFound();
            }

            return Ok(clientTask);
        }

     
        [HttpPost]
        public async Task<IActionResult> CreateClientTask(ClientTask clientTask)
        {
           
            try
            {
                foreach(var task in clientTask.Tasks)
                {
                    task.TaskId = GenerateUniqueId();
                }
                var createdClientTask = await _unitOfWork.ClientTaskRepository.AddAsync(clientTask);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex) { var err = ex.Message; }
          

            return Ok();
        }


        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(CreateTask task)
        {

            try
            {
                var clientTask = new ClientTask();
                clientTask.Client =  _unitOfWork.ClientRepository.GetAllAsync().Result.Where(w => w.ClientId == task.ClientId).FirstOrDefault();
                var Tasks = new List<Tasks>();
                var Task = new Tasks()
                {
                    TaskId = GenerateUniqueId(),
                    Name = task.TaskName
                };
                Tasks.Add(Task);
                clientTask.Tasks = Tasks;
                var createdClientTask = await _unitOfWork.ClientTaskRepository.AddAsync(clientTask);
                await _unitOfWork.SaveChangesAsync();
                return Ok(createdClientTask);
            }
            catch (Exception ex) { 
                var err = ex.Message;
                return BadRequest(err);
            }
             
        }

        private int GenerateUniqueId()
        {
            // Logic to generate a unique ClientId (4-6 digits)
            Random random = new Random();
            return random.Next(1000, 1000000); // Adjust range as needed
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientTask clientTask)
        {
            if (id != clientTask.Id)
            {
                return BadRequest();
            }
            // Get the original clientTask with tasks from the database
            var originalClientTask = await _unitOfWork.ClientTaskRepository.GetByIdAsync(clientTask.Id, includeProperties: ct => ct.Tasks);

            // Update scalar properties of clientTask (e.g., properties other than the Tasks navigation property)
            // ...

            // Update the tasks
            var finalTasks = UpdateTasks(originalClientTask.Tasks, clientTask.Tasks);
            originalClientTask.Tasks = finalTasks;
            var updatedClientTask = await _unitOfWork.ClientTaskRepository.UpdateAsync(originalClientTask);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedClientTask);
        }

        private List<Tasks> UpdateTasks(List<Tasks> existingTasks, List<Tasks> updatedTasks)
        {
            // Remove tasks that are not present in the updatedTasks and are not part of existingTasks
            existingTasks.RemoveAll(et => !updatedTasks.Any(ut => ut.Id == et.Id) && !existingTasks.Contains(et));
            var finalTasks = new List<Tasks>();
            // Update or add tasks
            foreach (var updatedTask in updatedTasks)
            {
                var existingTask = existingTasks.FirstOrDefault(et => et.Id == updatedTask.Id);
             
                if (existingTask != null)
                {
                    // Update existing task
                    existingTask.Name = updatedTask.Name;
                    finalTasks.Add(existingTask);
                    // Update other properties as needed
                }
                else
                {
                    // Add new task
                    updatedTask.TaskId = GenerateUniqueId();
                    finalTasks.Add(updatedTask);
                }

            }

            return finalTasks;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientTask(int id)
        {
            var deleted = await _unitOfWork.ClientTaskRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
