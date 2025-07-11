using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.Extensions.Logging;

public class ApiResponseProcessorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApiResponseProcessorService> _logger;

    public ApiResponseProcessorService(IServiceProvider serviceProvider, ILogger<ApiResponseProcessorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Get all machines (or filter if needed)
                    var allMachines = await unitOfWork.MachineRepository.GetAllAsync();

                    foreach (var machine in allMachines)
                    {
                        if (!string.IsNullOrWhiteSpace(machine.ApiResponse))
                        {
                            machine.Status = true;
                            machine.ApiResponse = string.Empty;
                        }
                        else
                        {
                            machine.Status = false;
                        }

                        await unitOfWork.MachineRepository.UpdateAsync(machine);
                    }

                    if (allMachines.Any())
                    {
                        await unitOfWork.SaveChangesAsync();
                        _logger.LogInformation($"{allMachines.Count()} machines processed at {DateTime.Now}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing ApiResponse in background service.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

}
