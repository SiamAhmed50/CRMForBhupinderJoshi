using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using System.Linq.Expressions;
using CRM.API.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CRM.Data.Enums;

namespace CRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientTaskController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientTaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetClientTasks()
        {
            var clientTasks = await _unitOfWork.ClientTaskRepository.GetAllAsync(
                includes: new Expression<Func<ClientTask, object>>[]
                    {
                        ct => ct.Client,
                        ct => ct.Tasks
                    });
            return Ok(clientTasks);
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientTask(int id)
        {
            var clientTask = await _unitOfWork.ClientTaskRepository.GetByIdAsync(ct => EF.Property<int>(ct, "Id") == id);


            if (clientTask == null)
            {
                return NotFound();
            }

            return Ok(clientTask);
        }

     
        [HttpPost]
        public async Task<IActionResult> CreateClientTask(ClientTask clientTask)
        {

            var createdClientTask = await _unitOfWork.ClientTaskRepository.AddAsync(clientTask);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetClientTask", new { id = createdClientTask.Id }, createdClientTask);
        }
        [HttpPost("CreateTask")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateClientTask(CreateTask task)
        {

            var client =  _unitOfWork.ClientRepository.GetAllAsync(w => w.ClientId == task.ClientId).Result.FirstOrDefault();
            if(client != null)
            {
                if (client.LicenseStatus && client.LicenseEndDate > DateTime.UtcNow)
                {
                    var newTasks = new List<Tasks>();
                    foreach (var taskName in task.TaskNames)
                    {
                        var newTask = new Tasks();
                        newTask.Name = taskName;
                        newTasks.Add(newTask);
                    }
                    var clientbj = new Client()
                    {
                        Id = client.Id
                    };
                    var Task = new ClientTask()
                    {
                        Client = client,
                        ClientId = task.ClientId,
                        Tasks = newTasks
                    };
                    var createdClientTask = await _unitOfWork.ClientTaskRepository.AddAsync(Task);
                    await _unitOfWork.SaveChangesAsync();

                    return CreatedAtAction("GetClientTask", new { id = createdClientTask.Id }, createdClientTask);
                }
                else
                {
                    return BadRequest("Client Licesne got expired!");
                }
            }

            return NotFound("Client Not found!");
         
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientTask clientTask)
        {
            if (id != clientTask.Id)
            {
                return BadRequest();
            }

            var updatedClientTask = await _unitOfWork.ClientTaskRepository.UpdateAsync(clientTask);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedClientTask);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.TaskRepository.GetAllAsync(filter:x=>x.ClientTaskId==id);
            foreach(var item in data)
            {
                await _unitOfWork.TaskRepository.DeleteAsync(item.Id.ToString());
            }
            var deleted = await _unitOfWork.ClientTaskRepository.DeleteAsync(id.ToString());

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("ToggleTaskStatus/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ToggleTaskStatus(int id)
        {
            // Retrieve the client task by ID
            var clientTask = await _unitOfWork.ClientTaskRepository.GetByIdAsync(ct => EF.Property<int>(ct, "Id") == id);

            if (clientTask == null)
            {
                return NotFound(new { success = false, message = "Client task not found." });
            }

            // Toggle the task status (assuming 0 = Idle, 1 = Running)
            if (clientTask.Status == ClientTaskStatus.Idle)
            {
                clientTask.Status = ClientTaskStatus.Running;
            }
            else if (clientTask.Status == ClientTaskStatus.Running)
            {
                clientTask.Status = ClientTaskStatus.Idle;
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid task status." });
            }

            // Update the task in the repository
            var updatedClientTask = await _unitOfWork.ClientTaskRepository.UpdateAsync(clientTask);
            await _unitOfWork.SaveChangesAsync();

            // Return the updated status
            return Ok(new { success = true, status = clientTask.Status });
        }

        [HttpGet("RunningTasks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRunningTasks()
        {
            try
            {
                // Fetch all client tasks with status `Running`
                var runningTasks = await _unitOfWork.ClientTaskRepository.GetAllAsync(
                    filter: ct => ct.Status == ClientTaskStatus.Running,
                    includes: new Expression<Func<ClientTask, object>>[]
                    {
                ct => ct.Client,
                ct => ct.Tasks
                    });

                if (runningTasks == null || !runningTasks.Any())
                {
                    return NotFound(new { success = false, message = "No running tasks found." });
                }

                return Ok(new { success = true, data = runningTasks.FirstOrDefault() });
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new { success = false, message = "An error occurred while processing the request." });
            }
        }

    }
}
