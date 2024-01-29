using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class JobLogsService : Repository<JobLogs, int>, IJobLogsRepository
    {
        public JobLogsService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
        

    }
}
