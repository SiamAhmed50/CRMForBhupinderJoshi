using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;

namespace CRM.Controllers
{
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
            var clientTasks = await _unitOfWork.ClientTaskRepository.GetAllAsync();
            return Ok(clientTasks);
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
            var createdClientTask = await _unitOfWork.ClientTaskRepository.AddAsync(clientTask);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetClientTask", new { id = createdClientTask.Id }, createdClientTask);
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
