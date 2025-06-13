using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Services.Repositories
{
    public class UserClientService : IUserClientRepository
    {
        private readonly ProjectDbContext _context;
        public UserClientService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetClientsByUserIdAsync(string userId)
        {
            return await _context.UserClients
                .Where(um => um.UserId == userId)
                .Select(um => um.Client)
                .ToListAsync();
        }
 
        public async Task AddUserClientsAsync(string userId, List<int> menuIds)
        {
            var userClients = menuIds.Select(menuId => new UserClients
            {
                UserId = userId,
                ClientId = menuId
            }).ToList();

            _context.UserClients.AddRange(userClients);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserClientsAsync(string userId)
        {
            var existing = _context.UserClients.Where(x => x.UserId == userId);
            _context.UserClients.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }
    }

}
