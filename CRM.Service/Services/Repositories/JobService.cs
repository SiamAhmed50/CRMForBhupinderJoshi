using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class JobService : Repository<Job, int>, IJobRepository
    {
        public JobService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
