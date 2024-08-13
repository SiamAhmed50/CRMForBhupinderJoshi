using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class ClientTaskService : Repository<ClientTask, int>, IClientTaskRepository
    {
        public ClientTaskService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
