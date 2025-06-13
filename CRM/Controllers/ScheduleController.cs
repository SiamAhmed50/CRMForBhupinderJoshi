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
    public class ScheduleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScheduleController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/Schedules
        [HttpGet]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            try
            {
                // 1) Load the current user (including their assigned clients)
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.Users
                    .Include(u => u.UserClients)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (currentUser == null)
                    return Unauthorized();

                // 2) Check if they’re in the Admin role
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                // 3) Define your Includes
                Expression<Func<Schedule, object>>[] includes = {
            s => s.Client,
            s => s.ClientTask,
            s => s.ClientTask.Tasks
        };

                IEnumerable<Schedule> schedules;

                if (isAdmin)
                {
                    // Admins see all schedules
                    schedules = await _unitOfWork.ScheduleRepository
                        .GetAllAsync(filter: null, includes: includes);
                }
                else
                {
                    // Non-admins see only schedules for their clients
                    var allowedClientIds = currentUser.UserClients
                        .Select(uc => uc.ClientId)
                        .ToList();

                    schedules = await _unitOfWork.ScheduleRepository
                        .GetAllAsync(
                            filter: s => allowedClientIds.Contains(s.ClientId),
                            includes: includes
                        );
                }

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/Schedules/1
        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(
                s => EF.Property<int>(s, "Id") == id,
                includes: new Expression<Func<Schedule, object>>[]
                {
                    s => s.Client,
                    s => s.ClientTask
                });

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        // POST: api/Schedules
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateSchedule(Schedule schedule)
        {
            if (schedule.ClientId != 0)
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(c => EF.Property<int>(c, "Id") == schedule.ClientId);
                if (client != null)
                {
                    schedule.Client = client;
                }
            }

            if (schedule.ClientTaskId !=0)
            {
                var clientTask = await _unitOfWork.ClientTaskRepository.GetByIdAsync(ct => EF.Property<int>(ct, "Id") == schedule.ClientTaskId);
                if (clientTask != null)
                {
                    schedule.ClientTask = clientTask;
                }
            }

            var createdSchedule = await _unitOfWork.ScheduleRepository.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetSchedule", new { id = createdSchedule.Id }, createdSchedule);
        }

        // PUT: api/Schedules/5
        [HttpPut("{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> UpdateSchedule(int id, Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return BadRequest();
            }

            var updatedSchedule = await _unitOfWork.ScheduleRepository.UpdateAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return Ok(updatedSchedule);
        }

        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var deleted = await _unitOfWork.ScheduleRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
