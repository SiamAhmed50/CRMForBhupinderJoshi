using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MachineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Machines
        [HttpGet]
        public async Task<IActionResult> GetMachines()
        {
            try
            {
                var Machines = await _unitOfWork.MachineRepository.GetAllAsync(
                    includes: new Expression<Func<Machine, object>>[]
                    {
                        ct => ct.Client
                    });
                return Ok(Machines);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return Ok();
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
               var client = await _unitOfWork.ClientRepository.GetByIdAsync(c => EF.Property<int>(c, "ClientId") == machine.ClientId);
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

        // DELETE: api/Machines/5
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
        }
    }
}
