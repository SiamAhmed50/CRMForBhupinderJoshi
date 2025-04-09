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
    public class UserMenuService : IUserMenuRepository
    {
        private readonly ProjectDbContext _context;
        public UserMenuService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menus>> GetMenusByUserIdAsync(string userId)
        {
            return await _context.UserMenus
                .Where(um => um.UserId == userId)
                .Select(um => um.Menu)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menus>> GetMenusAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task AddUserMenusAsync(string userId, List<int> menuIds)
        {
            var userMenus = menuIds.Select(menuId => new UserMenus
            {
                UserId = userId,
                MenuId = menuId
            }).ToList();

            _context.UserMenus.AddRange(userMenus);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserMenusAsync(string userId)
        {
            var existing = _context.UserMenus.Where(x => x.UserId == userId);
            _context.UserMenus.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }
    }

}
