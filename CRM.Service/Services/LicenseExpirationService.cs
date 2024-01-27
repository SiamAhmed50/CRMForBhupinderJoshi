using CRM.Data.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Services
{
    public class LicenseExpirationService : IHostedService, IDisposable
    {
        private readonly ILogger<LicenseExpirationService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LicenseExpirationService(ILogger<LicenseExpirationService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("LicenseExpirationService is starting.");

            // Start a background task to check license expirations
            Task.Run(() => CheckLicenseExpirations(cancellationToken), cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("LicenseExpirationService is stopping.");

            return Task.CompletedTask;
        }

        private async Task CheckLicenseExpirations(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>(); // Replace with your actual DbContext type
                    var currentDate = DateTime.UtcNow;

                    // Get licenses that are still active and have end dates before or equal to the current date
                    var expiredLicenses = dbContext.Clients
                        .Where(c => c.LicenseStatus && c.LicenseEndDate <= currentDate)
                        .ToList();

                    // Update license status to expired for expired licenses
                    foreach (var expiredLicense in expiredLicenses)
                    {
                        expiredLicense.LicenseStatus = false;
                    }

                    dbContext.SaveChanges();
                }

                // Check every 24 hours (adjust as needed)
                await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
            }
        }

        public void Dispose()
        {
            // Dispose any resources if needed
        }
    }
}
