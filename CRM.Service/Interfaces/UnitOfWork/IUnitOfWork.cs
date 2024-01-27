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
        IRepository<ApplicationUser, string> GenericUserRepository { get; }
        IRepository<Client, string> GenericClientRepository { get; } 
        IRepository<ClientTask, string> GenericClientTaskRepository { get; } 
        IRepository<Task, string> GenericTaskRepository { get; }

        IClientRepository ClientRepository { get; }
        IClientTaskRepository ClientTaskRepository { get; }
        // Add other repositories here

        Task<int> SaveChangesAsync();
    }

}
