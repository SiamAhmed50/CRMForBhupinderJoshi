using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CRM.Service.Interfaces.UnitOfWork;

public class LicenseStatusUpdaterService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public LicenseStatusUpdaterService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateLicenseStatuses();
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task UpdateLicenseStatuses()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var clients = await unitOfWork.ClientRepository.GetAllAsync();
            var today = DateTime.Today;

            foreach (var client in clients)
            {
                if (client.LicenseEndDate < today && client.LicenseStatus)
                {
                    client.LicenseStatus = false;
                    await unitOfWork.ClientRepository.UpdateAsync(client);
                }
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}
