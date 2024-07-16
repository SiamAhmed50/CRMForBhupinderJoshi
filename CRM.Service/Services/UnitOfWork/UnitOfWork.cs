using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using CRM.Service.Interfaces.UnitOfWork;
using CRM.Service.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDbContext _dbContext;

        public UnitOfWork(ProjectDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            UserRepository = new Repository<ApplicationUser, string>(_dbContext);
            TaskRepository = new Repository<Tasks, string>(_dbContext);
            ClientRepository = new Repository<Client, string>(_dbContext); // Add this line for Client Repository
            ClientTaskRepository = new Repository<ClientTask, string>(_dbContext); 
            JobLogsRepository = new Repository<JobLogs, string>(_dbContext); 
            LogsRepository = new Repository<Logs, string>(_dbContext); 
        }

        public IRepository<ApplicationUser, string> UserRepository { get; }
        public IRepository<Client, string> ClientRepository { get; } // Add this property for Client Repository
        public IRepository<ClientTask, string> ClientTaskRepository { get; } // Add this property for Client Repository
        public IRepository<Job, string> JobRepository { get; }
        public IRepository<JobLogs, string> GenericJobLogsRepository { get; }
        public IRepository<Logs, string> LogsRepository { get; }
        public IRepository<JobLogs, string> JobLogsRepository { get; }
        public IRepository<Tasks, string> TaskRepository { get; }


        // Add other repositories here

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
