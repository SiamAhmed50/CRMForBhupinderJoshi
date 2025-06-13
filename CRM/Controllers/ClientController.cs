using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientsController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                // 1) Load the current user from the store, including their UserClients
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.Users
                    .Include(u => u.UserClients)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (currentUser == null)
                    return Unauthorized();

                // 2) Check if they're in the Admin role
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                IEnumerable<Client> clients;

                if (isAdmin)
                {
                    // Admins see every client
                    clients = await _unitOfWork.ClientRepository.GetAllAsync();
                }
                else
                {
                    // Non-admins only see their assigned client(s)
                    var allowedClientIds = currentUser.UserClients
                                                    .Select(uc => uc.ClientId)
                                                    .ToList();

                    clients = await _unitOfWork.ClientRepository
                        .GetAllAsync(filter: c => allowedClientIds.Contains(c.Id));
                }

                return Ok(clients);
            }
            catch (Exception ex)
            {
                // log ex.Message if you have a logger
                return StatusCode(500, "Internal server error");
            }
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

        // POST: api/Clients/5/delete
        [HttpPost("{id}/delete")]
        [AllowAnonymous]    // if you still want anonymous access
        public async Task<IActionResult> DeleteClientViaPost(int id)
        {
            var deleted = await _unitOfWork.ClientRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            await _unitOfWork.SaveChangesAsync();
            return NoContent();   // 204
        }

    }
}
