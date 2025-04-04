using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Interfaces.Repositories
{
    public interface IUserMenuRepository
    {
        Task<IEnumerable<Menus>> GetMenusByUserIdAsync(string userId);
        Task AddUserMenusAsync(string userId, List<int> menuIds);
        Task RemoveUserMenusAsync(string userId);
    }

}
