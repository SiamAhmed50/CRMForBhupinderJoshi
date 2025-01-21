using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Enums;

namespace CRM.Data.Entities
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [ForeignKey(nameof(ClientTask))]
        public int ClientTaskId { get; set; }
        public ClientTask? ClientTask { get; set; }
        public string TimeZone { get; set; }
        public ScheduleType ScheduleType { get; set; }

        // Daily schedule specific properties
        public int? DailyHour { get; set; }
        public int? DailyMinute { get; set; }
        //public DayOfWeek? DayOfWeek { get; set; }

        // Weekly schedule specific properties
        public List<DayOfWeek>? DayOfWeek { get; set; }
        // Custom cron expression
        public string? CronExpression { get; set; }

        
    }
}
