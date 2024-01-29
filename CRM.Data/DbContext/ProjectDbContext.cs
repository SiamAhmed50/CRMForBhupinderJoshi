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
        public DbSet<Jobs> Jobs { get; set; } // Add this DbSet for the Client entity
        public DbSet<Logs> Logs { get; set; } // Add this DbSet for the Client entity
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<Jobs>()
            .HasOne(j => j.Client)
            .WithMany()
            .HasForeignKey(j => j.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Jobs>()
                .HasOne(j => j.Tasks)
                .WithMany()
                .HasForeignKey(j => j.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            // Explicitly configure foreign keys with ON DELETE NO ACTION for JobLogs
            modelBuilder.Entity<JobLogs>()
                .HasOne(jl => jl.Client)
                .WithMany()
                .HasForeignKey(jl => jl.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobLogs>()
                .HasOne(jl => jl.Task)
                .WithMany()
                .HasForeignKey(jl => jl.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            // Explicitly configure foreign keys with ON DELETE NO ACTION for Logs
            modelBuilder.Entity<Logs>()
                .HasOne(l => l.JobLogs)
                .WithMany(jl => jl.Logs)
                .HasForeignKey(l => l.JobLogsId)
                .OnDelete(DeleteBehavior.NoAction);
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
