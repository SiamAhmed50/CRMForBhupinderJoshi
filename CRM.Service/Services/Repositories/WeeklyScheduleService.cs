using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class WeeklyScheduleService : Repository<WeeklySchedule, int>, IWeeklyScheduleRepository
    {
        public WeeklyScheduleService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
