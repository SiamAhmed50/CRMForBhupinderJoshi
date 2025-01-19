using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class ScheduleService : Repository<Schedule, int>, IScheduleRepository
    {
        public ScheduleService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
