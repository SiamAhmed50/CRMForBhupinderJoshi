using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CRM.Data.DbContext;
using CRM.Data.Enums;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

public class ScheduleWorker : IHostedService, IDisposable
{
    private readonly ILogger<ScheduleWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;

    public ScheduleWorker(ILogger<ScheduleWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Schedule Worker started.");
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }
    private async void ExecuteTask(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();

        try
        {
            var serverTimeZone = TimeZoneInfo.Local;
            var serverTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, serverTimeZone);

            // Fetch schedules from the database
            var schedules = await dbContext.Schedules
                .Include(s => s.ClientTask)
                .ToListAsync();

            foreach (var schedule in schedules)
            {
                // Convert display name to time zone ID
                var scheduleTimeZone = TimeZoneInfo.GetSystemTimeZones()
                    .FirstOrDefault(tz => tz.DisplayName == schedule.TimeZone);
                var scheduleCurrentTime = TimeZoneInfo.ConvertTime(serverTime, serverTimeZone, scheduleTimeZone);

                if (scheduleTimeZone == null)
                {
                    _logger.LogWarning($"Invalid time zone: {schedule.TimeZone} for schedule ID: {schedule.Id}");
                    continue;
                }

                DateTime scheduleTime;
                if (schedule.ScheduleType == ScheduleType.Daily)
                {
                    scheduleTime = new DateTime(
                        serverTime.Year,
                        serverTime.Month,
                        serverTime.Day,
                        schedule.DailyHour ?? 0,
                        schedule.DailyMinute ?? 0,
                        0
                    );
                }
                

                else if (schedule.ScheduleType == ScheduleType.Weekly && schedule.DayOfWeek != null)
                {
                    if (!schedule.DayOfWeek.Contains(scheduleCurrentTime.DayOfWeek))
                        continue;

                    scheduleTime = new DateTime(
                        serverTime.Year,
                        serverTime.Month,
                        serverTime.Day,
                        schedule.DailyHour ?? 0,
                        schedule.DailyMinute ?? 0,
                        0
                    );
                }
                /*else if (schedule.ScheduleType == ScheduleType.Custom && !string.IsNullOrEmpty(schedule.CronExpression))
                {
                    try
                    {
                        // Parse and evaluate Cron expression
                        var cronSchedule = CrontabSchedule.Parse(schedule.CronExpression);
                        var nextOccurrenceInScheduleTimeZone = cronSchedule.GetNextOccurrence(scheduleCurrentTime);
                        scheduleTime = nextOccurrenceInScheduleTimeZone;

                        //var nextOccurrenceInScheduleTimeZone = cronSchedule.GetNextOccurrence(serverTime.AddSeconds(-30) AddMinutes(-5), serverTime.AddMinutes(5));

                        if (nextOccurrenceInScheduleTimeZone != DateTime.MinValue)
                        {
                            scheduleTime = new DateTime(
                                nextOccurrenceInScheduleTimeZone.Year,
                                nextOccurrenceInScheduleTimeZone.Month,
                                nextOccurrenceInScheduleTimeZone.Day,
                                nextOccurrenceInScheduleTimeZone.Hour,
                                nextOccurrenceInScheduleTimeZone.Minute,
                                0
                            );
                        }
                        else
                        {
                            continue;
                        }
                        // Get the next occurrence directly from the Cron expression


                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Invalid Cron expression for schedule ID: {schedule.Id}");
                        continue;
                    }
                }*/

                /*else if (schedule.ScheduleType == ScheduleType.Custom && !string.IsNullOrEmpty(schedule.CronExpression))
                {
                    try
                    {
                        var normalizedScheduleCurrentTime = new DateTime(
                        scheduleCurrentTime.Year,
                        scheduleCurrentTime.Month,
                        scheduleCurrentTime.Day,
                        scheduleCurrentTime.Hour,
                        scheduleCurrentTime.Minute,
                        0, // Seconds set to 0
                        scheduleCurrentTime.Kind
);
                        // Parse and evaluate Cron expression
                        var cronSchedule = CrontabSchedule.Parse(schedule.CronExpression);

                        // Get the next occurrence from the Cron expression
                        var nextOccurrenceInScheduleTimeZone = cronSchedule.GetNextOccurrence(normalizedScheduleCurrentTime);
                        // Log for debugging
                        _logger.LogWarning($"Evaluating Cron expression for schedule ID {schedule.Id}");
                        _logger.LogWarning($"Next occurrence in schedule time zone: {nextOccurrenceInScheduleTimeZone}");
                        _logger.LogWarning($"Current schedule time: {scheduleCurrentTime}");
                        // Check if day of month, month, and day of week match
                        if (nextOccurrenceInScheduleTimeZone.Day != scheduleCurrentTime.Day ||
                            nextOccurrenceInScheduleTimeZone.Month != scheduleCurrentTime.Month ||
                            nextOccurrenceInScheduleTimeZone.DayOfWeek != scheduleCurrentTime.DayOfWeek)

                        {
                            // Log for debugging
                            _logger.LogWarning($"Schedule ID {schedule.Id} does not match current day, month, or day of week.");
                            continue;
                        } 


                        scheduleTime = new DateTime(
                        nextOccurrenceInScheduleTimeZone.Year,
                        nextOccurrenceInScheduleTimeZone.Month,
                        nextOccurrenceInScheduleTimeZone.Day,
                        nextOccurrenceInScheduleTimeZone.Hour,
                        nextOccurrenceInScheduleTimeZone.Minute,
                        0

                        );
                        _logger.LogWarning($"Schedule ID {schedule.Id}: scheduleTime assigned as {scheduleTime}");
                    }
                    
                    // Assign the next occurrence to scheduleTime
                    //scheduleTime = nextOccurrenceInScheduleTimeZone;


                    //}
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Invalid Cron expression for schedule ID: {schedule.Id}");
                        //scheduleTime = DateTime.MinValue; // Ensure no invalid schedule time is used
                        continue;
                    }
                }
*/
                else if (schedule.ScheduleType == ScheduleType.Custom && !string.IsNullOrEmpty(schedule.CronExpression))
                {
                    try
                    {
                        // Parse the Cron expression
                        var cronSchedule = CrontabSchedule.Parse(schedule.CronExpression);

                        // Normalize scheduleCurrentTime to match Cron's expected format (zero seconds)
                        var normalizedScheduleCurrentTime = new DateTime(
                            scheduleCurrentTime.Year,
                            scheduleCurrentTime.Month,
                            scheduleCurrentTime.Day,
                            scheduleCurrentTime.Hour,
                            scheduleCurrentTime.Minute,
                            0,
                            scheduleCurrentTime.Kind // Retain the DateTimeKind
                        );

                        // Check if current time already matches the Cron expression
                        var lastOccurrence = cronSchedule.GetNextOccurrence(normalizedScheduleCurrentTime.AddSeconds(-30), normalizedScheduleCurrentTime.AddSeconds(30));

                        if (lastOccurrence == normalizedScheduleCurrentTime)
                        {
                            // If the current time matches, use it directly as scheduleTime
                            scheduleTime = normalizedScheduleCurrentTime;
                            _logger.LogInformation($"Exact match found for schedule ID {schedule.Id} at {scheduleTime}");
                        }
                        else
                        {
                            _logger.LogInformation($"Current time does not match the Cron schedule for schedule ID {schedule.Id}. Skipping.");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Invalid Cron expression for schedule ID: {schedule.Id}");
                        continue;
                    }
                }

                else
                {
                    continue;
                }

                // Convert schedule time to server time
                var convertedScheduleTime = TimeZoneInfo.ConvertTime(scheduleTime, scheduleTimeZone, serverTimeZone);

                if (convertedScheduleTime.Hour == serverTime.Hour && convertedScheduleTime.Minute == serverTime.Minute)
                { 
                    // Mark the associated ClientTask as Running
                    if (schedule.ClientTask != null)
                    {
                        schedule.ClientTask.Status = ClientTaskStatus.Running;
                        _logger.LogInformation($"ClientTask {schedule.ClientTaskId} is set to Running.");
                    }
                }
                /* if (convertedScheduleTime.Hour == serverTime.Hour && convertedScheduleTime.Minute == serverTime.Minute)
                 {
                     if (schedule.ScheduleType == ScheduleType.Custom)
                     {
                         // Check day, month, and day of week for custom schedules
                         if (convertedScheduleTime.Day == serverTime.Day &&
                             convertedScheduleTime.Month == serverTime.Month &&
                             convertedScheduleTime.DayOfWeek == serverTime.DayOfWeek)
                         {
                             // Mark the associated ClientTask as Running
                             if (schedule.ClientTask != null)
                             {
                                 schedule.ClientTask.Status = ClientTaskStatus.Running;
                                 _logger.LogInformation($"Custom schedule ClientTask {schedule.ClientTaskId} is set to Running.");
                             }
                         }
                         else
                         {
                             _logger.LogInformation($"Custom schedule for ClientTask {schedule.ClientTaskId} does not match the server's day, month, or day of week.");
                         }
                     }
                     else
                     {
                         // For non-custom schedules, directly mark the status
                         if (schedule.ClientTask != null)
                         {
                             schedule.ClientTask.Status = ClientTaskStatus.Running;
                             _logger.LogInformation($"ClientTask {schedule.ClientTaskId} is set to Running.");
                         }
                     }
                 }*/


            }

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing schedules.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Schedule Worker stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
