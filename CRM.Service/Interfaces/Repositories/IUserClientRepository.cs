using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Interfaces.Repositories
{
    public interface IUserClientRepository
    {
        Task<IEnumerable<Client>> GetClientsByUserIdAsync(string userId); 
        Task AddUserClientsAsync(string userId, List<int> clientId);
        Task RemoveUserClientsAsync(string userId);
    }

}
