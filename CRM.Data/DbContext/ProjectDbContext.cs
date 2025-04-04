using CRM.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.DbContext
{
    public class ProjectDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Client> Clients { get; set; } // Add this DbSet for the Client entity
        public DbSet<ClientTask> ClientTasks { get; set; } // Add this DbSet for the Client entity
        public DbSet<Tasks> Tasks { get; set; } // Add this DbSet for the Client entity
        public DbSet<Job> Jobs { get; set; } // Add this DbSet for the Jobs entity
        public DbSet<JobLogs> JobLogs { get; set; } // Add this DbSet for the JobLogs entity
        public DbSet<Logs> Logs { get; set; } // Add this DbSet for the Logs entity
        public DbSet<Machine> Machines { get; set; } // Add this DbSet for the Machine entity
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<WeeklySchedule> WeeklySchedules { get; set; }

        public DbSet<JobTransactions> JobTransactions { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<UserMenus> UserMenus { get; set; }



        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<UserMenus>()
       .HasOne(um => um.User)
       .WithMany(u => u.UserMenus)
       .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMenus>()
                .HasOne(um => um.Menu)
                .WithMany()
                .HasForeignKey(um => um.MenuId);

            //--------------------------------------------------------------------
            // 1) CLIENT -> CLIENTTASK
            //--------------------------------------------------------------------
            // Each ClientTask has one ClientId (FK), so one Client can have many ClientTasks.
            // OnDelete: If a Client is deleted, all related ClientTasks are also deleted.
            modelBuilder.Entity<ClientTask>()
                .HasOne(ct => ct.Client)
                .WithMany()                     // or .WithMany(client => client.ClientTasks) if you add a navigation property
                .HasForeignKey(ct => ct.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 2) CLIENT -> MACHINE
            //--------------------------------------------------------------------
            modelBuilder.Entity<Machine>()
                .HasOne(m => m.Client)
                .WithMany()                    // or .WithMany(client => client.Machines)
                .HasForeignKey(m => m.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 3) CLIENT -> JOBLOGS
            //--------------------------------------------------------------------
            modelBuilder.Entity<JobLogs>()
                .HasOne(jl => jl.Client)
                .WithMany()                    // or .WithMany(client => client.JobLogs)
                .HasForeignKey(jl => jl.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 4) CLIENT -> JOB
            //--------------------------------------------------------------------
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Client)
                .WithMany()                    // or .WithMany(client => client.Jobs)
                .HasForeignKey(j => j.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 5) CLIENT -> SCHEDULE
            //--------------------------------------------------------------------
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Client)
                .WithMany()                    // or .WithMany(client => client.Schedules)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 6) CLIENTTASK -> TASKS
            //--------------------------------------------------------------------
            // One ClientTask can have multiple Tasks
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.ClientTask)
                .WithMany(ct => ct.Tasks)
                .HasForeignKey(t => t.ClientTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 7) TASKS -> JOB
            //--------------------------------------------------------------------
            // One Tasks record can be used by multiple Job records
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Tasks)
                .WithMany()                    // or .WithMany(t => t.Jobs) if you add that navigation
                .HasForeignKey(j => j.TasksId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 8) TASKS -> JOBLOGS
            //--------------------------------------------------------------------
            // One Tasks record can be used by multiple JobLogs
            modelBuilder.Entity<JobLogs>()
                .HasOne(jl => jl.Task)
                .WithMany()                    // or .WithMany(t => t.JobLogs)
                .HasForeignKey(jl => jl.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 9) JOB -> JOBLOGS
            //--------------------------------------------------------------------
            // One Job can have multiple JobLogs
            modelBuilder.Entity<JobLogs>()
                .HasOne(jl => jl.Job)
                .WithMany()                    // or .WithMany(j => j.JobLogs)
                .HasForeignKey(jl => jl.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 10) JOB -> JOBTRANSACTIONS
            //--------------------------------------------------------------------
            // One Job can have multiple JobTransactions
            modelBuilder.Entity<JobTransactions>()
                .HasOne<Job>()                 // If you add a navigation property, use .HasOne(jt => jt.Job)
                .WithMany()                    // or .WithMany(j => j.JobTransactions)
                .HasForeignKey(jt => jt.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 11) JOBLOGS -> LOGS
            //--------------------------------------------------------------------
            // One JobLogs can have multiple Logs
            modelBuilder.Entity<Logs>()
                .HasOne(l => l.JobLog)
                .WithMany(jl => jl.Logs)
                .HasForeignKey(l => l.JoblogId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------------------------------------------
            // 12) CLIENTTASK -> SCHEDULE
            //--------------------------------------------------------------------
            // One ClientTask can have multiple Schedules
            modelBuilder.Entity<Schedule>()
             .HasOne(s => s.ClientTask)
             .WithMany() // no property in ClientTask
             .HasForeignKey(s => s.ClientTaskId)
             .OnDelete(DeleteBehavior.Cascade);


            //--------------------------------------------------------------------
            // 13) SCHEDULE -> WEEKLYSCHEDULE
            //--------------------------------------------------------------------
            // One Schedule can have multiple WeeklySchedule entries
            modelBuilder.Entity<WeeklySchedule>()
                .HasOne(ws => ws.Schedule)
                .WithMany()                    // or .WithMany(s => s.WeeklySchedules)
                .HasForeignKey(ws => ws.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            // Seed admin user
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123")
            });

            // Assign roles to admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId = "1" } // Admin role assigned to admin user
            );
        }

    }

   
}
