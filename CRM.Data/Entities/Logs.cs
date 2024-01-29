using CRM.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Logs
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("JobLogsId")]
        public JobLogs? JobLogs { get; set; }

        // Foreign key for Client

        public int JobLogsId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string LogMessage { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
