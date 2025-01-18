using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class WeeklySchedule
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Schedule))]
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public DayOfWeek Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
