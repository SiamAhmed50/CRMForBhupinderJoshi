using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Interfaces.Repositories
{
    public interface IMenuService
    {
        Task<List<Menus>> GetMenusForCurrentUserAsync(string userId);
        Task<List<Menus>> GetAllMenusAsync();
    }
}
