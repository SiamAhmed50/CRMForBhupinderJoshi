using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class ClientService : Repository<Client, int>, IClientRepository
    {
        public ClientService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Client> GetByClientIdAndLicenseNumberAsync(int clientId, string licenseNumber)
        {
            return await GetDbSet().SingleOrDefaultAsync(c => c.ClientId == clientId && c.LicenseNumber == licenseNumber);
        }

    }
}
