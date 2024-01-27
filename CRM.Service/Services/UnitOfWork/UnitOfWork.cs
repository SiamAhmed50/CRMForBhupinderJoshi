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
            GenericUserRepository = new Repository<ApplicationUser, string>(_dbContext);
            GenericClientRepository = new Repository<Client, string>(_dbContext); // Add this line for Client Repository
            GenericClientTaskRepository = new Repository<ClientTask, string>(_dbContext); // Add this line for Client Repository
            GenericTaskRepository = new Repository<Task, string>(_dbContext); // Add this line for Client Repository

            // Initialize your specific repositories
            ClientRepository = new ClientService(_dbContext);
            ClientTaskRepository = new ClientTaskService(_dbContext);
        }

        public IRepository<ApplicationUser, string> GenericUserRepository { get; }
        public IRepository<Client, string> GenericClientRepository { get; } // Add this property for Client Repository
        public IRepository<ClientTask, string> GenericClientTaskRepository { get; } // Add this property for Client Repository
        public IRepository<Task, string> GenericTaskRepository { get; } // Add this property for Client Repository


        public IClientRepository ClientRepository { get; }
        public IClientTaskRepository ClientTaskRepository { get; }

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
