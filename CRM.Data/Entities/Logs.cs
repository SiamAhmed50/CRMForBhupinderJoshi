using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public int JoblogId { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
