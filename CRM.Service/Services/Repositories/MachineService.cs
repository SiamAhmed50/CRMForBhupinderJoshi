using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class MachineService : Repository<Machine, int>, IMachineRepository
    {
        public MachineService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
