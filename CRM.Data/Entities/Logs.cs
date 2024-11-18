using CRM.Data.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string LogMessage { get; set; }
        public int JoblogId { get; set; } // Make sure this matches your database column

        [ForeignKey(nameof(JoblogId))] // Explicitly specify the foreign key
        public JobLogs JobLog { get; set; }

        public LogType LogType { get; set; }
    }
}
