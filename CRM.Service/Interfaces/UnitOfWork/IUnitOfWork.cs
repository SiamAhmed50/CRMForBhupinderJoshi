using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ApplicationUser, string> UserRepository { get; }
        IRepository<Client, string> ClientRepository { get; } 
        IRepository<ClientTask, string> ClientTaskRepository { get; } 
        // Add other repositories here

        Task<int> SaveChangesAsync();
    }

}
