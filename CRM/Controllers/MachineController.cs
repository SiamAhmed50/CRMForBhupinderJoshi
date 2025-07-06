using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public MachineController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/Machines
        [HttpGet]
        public async Task<IActionResult> GetMachines()
        {
            try
            {
                // 1) Load current user with their assigned clients
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.Users
                    .Include(u => u.UserClients)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (currentUser == null)
                    return Unauthorized();

                // 2) Check Admin role
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                // 3) Define includes
                Expression<Func<Machine, object>>[] includes = {
                m => m.Client
            };

                IEnumerable<Machine> machines;

                if (isAdmin)
                {
                    // Admins see all machines
                    machines = await _unitOfWork.MachineRepository
                        .GetAllAsync(filter: null, includes: includes);
                }
                else
                {
                    // Non-admins see only machines for their clients
                    var allowedClientIds = currentUser.UserClients
                        .Select(uc => uc.ClientId)
                        .ToList();

                    machines = await _unitOfWork.MachineRepository
                        .GetAllAsync(
                            filter: m => allowedClientIds.Contains(m.ClientId),
                            includes: includes
                        );
                }

                return Ok(machines);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/Machines/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachine(int id)
        {
            var Machine = await _unitOfWork.MachineRepository.GetByIdAsync(c => EF.Property<int>(c, "Id") == id);


            if (Machine == null)
            {
                return NotFound();
            }

            return Ok(Machine);
        }



        /*// POST: api/Machines
        [HttpPost]
        public async Task<IActionResult> CreateMachine(Machine machine)
        {
            var createdMachine= await _unitOfWork.MachineRepository.AddAsync(machine);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetMachine", new { id = createdMachine.Id }, createdMachine);
        }*/

        [HttpPost]
        public async Task<IActionResult> CreateMachine(Machine machine)
        {
            if (machine.ClientId != 0)
            {
               var client = await _unitOfWork.ClientRepository.GetByIdAsync(c => EF.Property<int>(c, "Id") == machine.ClientId);
               if (client != null)
               {
                   machine.Client = client;
               }
            }

            var createdMachine = await _unitOfWork.MachineRepository.AddAsync(machine);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetMachine", new { id = createdMachine.Id }, createdMachine);
        }


        // PUT: api/Machines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, Machine machine)
        {
            if (id != machine.Id)
            {
                return BadRequest();
            }

            var updatedMachine = await _unitOfWork.MachineRepository.UpdateAsync(machine);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedMachine);
        }

        /*// DELETE: api/Machines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var deleted = await _unitOfWork.MachineRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }*/

        // POST: api/Machines/5/delete
        [HttpPost("{id}/delete")]
        [AllowAnonymous]    // if you still want anonymous access
        public async Task<IActionResult> DeleteMachineViaPost(int id)
        {
            var deleted = await _unitOfWork.MachineRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            await _unitOfWork.SaveChangesAsync();
            return NoContent();   // 204
        }

        // GET: api/Machines/ByClientId/1
        [HttpGet("ByClientId/{clientId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMachinesByClientId(int clientId)
        {
            try
            {
                var machines = await _unitOfWork.MachineRepository.GetAllAsync(
                    filter: m => m.ClientId == clientId,
                    includes: new Expression<Func<Machine, object>>[]
                    {
                m => m.Client
                    });

                if (machines == null || !machines.Any())
                {
                    return NotFound();
                }

                return Ok(machines);
            }
            catch (Exception ex)
            {
                // Log the exception details if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UpdateStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStatus(int clientId, string machineIp, bool status)
        {
            try
            {
                var machine = await _unitOfWork.MachineRepository.GetByIdAsync(m =>
                    m.ClientId == clientId && m.MachineIp.Contains(machineIp));

                if (machine == null)
                    return NotFound();

                machine.Status = status;
                await _unitOfWork.MachineRepository.UpdateAsync(machine);
                await _unitOfWork.SaveChangesAsync();

                return Ok(machine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
