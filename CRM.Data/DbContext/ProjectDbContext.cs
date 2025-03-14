﻿using CRM.Data.Entities;
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


        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

    public class ApplicationUser : IdentityUser
    {
        // You can add additional properties for your user model here
    }
}
