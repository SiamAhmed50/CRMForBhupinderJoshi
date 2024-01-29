using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class LogsService : Repository<Logs, int>, ILogsRepository
    {
        public LogsService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
        

    }
}
