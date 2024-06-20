using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class IClientTaskService : Repository<ClientTask, int>, IClientTaskRepository
    {
        public IClientTaskService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
