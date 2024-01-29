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
        IRepository<Jobs, string> GenericJobRepository { get; }
        IRepository<Logs, string> GenericLogsRepository { get; } 
        IRepository<JobLogs, string> GenericJobLogsRepository { get; } 

        IClientRepository ClientRepository { get; }
        IClientTaskRepository ClientTaskRepository { get; } 
        IJobRepository JobRepository { get; } 
        ILogsRepository LogsRepository { get; } 
        IJobLogsRepository JobLogsRepository { get; } 
        ITaskRepository TaskRepository { get; } 
        // Add other repositories here

        Task<int> SaveChangesAsync();
    }

}
