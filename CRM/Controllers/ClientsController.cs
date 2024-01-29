using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;

namespace CRM.Controllers
{

    //[Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _unitOfWork.ClientRepository.GetAllAsync();
            return Ok(clients);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<IActionResult> CreateClient(Client client)
        {
            var createdClient = await _unitOfWork.ClientRepository.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = createdClient.Id }, createdClient);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var updatedClient = await _unitOfWork.ClientRepository.UpdateAsync(client);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedClient);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var deleted = await _unitOfWork.ClientRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Clients/ByClientIdAndLicenseNumber?clientId=1&licenseNumber=XYZ123
        [HttpGet("ByClientIdAndLicenseNumber")]
        [AllowAnonymous] // Allow anonymous access for this specific API
        public async Task<IActionResult> GetClientByClientIdAndLicenseNumber(int clientId, string licenseNumber)
        {
            var client = await _unitOfWork.ClientRepository.GetByClientIdAndLicenseNumberAsync(clientId, licenseNumber);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }
    }
}
