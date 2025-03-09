using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Authorize]
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
            try
            {
                var clients = await _unitOfWork.ClientRepository.GetAllAsync();
                return Ok(clients);
            }
            catch (Exception ex) { 
            var message = ex.Message;
            }
           
            return Ok();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(c => EF.Property<int>(c, "Id") == id);


            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // GET: api/Clients/ClientId/{clientId}
        [HttpGet("ClientId/{clientId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClientByClientId(int clientId)
        {
            var client = await _unitOfWork.ClientRepository.GetAllAsync(filter: x => x.ClientCode == clientId);

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
        [AllowAnonymous]
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
    }
}
