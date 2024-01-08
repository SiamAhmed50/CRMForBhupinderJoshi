using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class ClientRepository : Repository<Client, int>, IClientRepository
    {
        public ClientRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
