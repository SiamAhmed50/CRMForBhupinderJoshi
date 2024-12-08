using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CRM.Data.DbContext
{
    public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public ProjectDbContext CreateDbContext(string[] args)
        {
            // Create the configuration object to read the connection string
            var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json") // The file should be in the root directory
				.Build();

            // Build the DbContextOptions with the connection string
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnectionString"));

            return new ProjectDbContext(optionsBuilder.Options);
        }
    }
}
