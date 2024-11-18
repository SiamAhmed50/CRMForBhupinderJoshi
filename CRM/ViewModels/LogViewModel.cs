using CRM.Data.Entities;
using CRM.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.API.ViewModels
{
    public class LogViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string LogMessage { get; set; }
        public int JoblogId { get; set; } // Make sure this matches your database column  
        public string LogType { get; set; }
    }
}
