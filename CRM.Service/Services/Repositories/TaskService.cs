using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Services.Repositories
{
    public class TaskService : Repository<Task, int>, ITaskRepository
    {
        public TaskService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
