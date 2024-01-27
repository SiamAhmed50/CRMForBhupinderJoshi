using CRM.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Logs
    {
        [Key]
        public int Id { get; set; }

        public Jobs? Job { get; set; }

        // Foreign key for Client
        public int JobId { get; set; } 
        public DateTime? Timestamp { get; set; }
        public string LogMessage { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
