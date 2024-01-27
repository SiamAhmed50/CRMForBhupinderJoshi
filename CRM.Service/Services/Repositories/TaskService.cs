using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Services.Repositories
{
    public class TaskService : Repository<Task, int>, ITaskRepository
    {
        public TaskService(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}
