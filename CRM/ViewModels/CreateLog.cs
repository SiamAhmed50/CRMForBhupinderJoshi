using CRM.Data.Enums;

namespace CRM.API.ViewModels
{
    public class CreateLog
    {
        public int JobId { get; set; }

        public DateTime Timestamp { get; set; }

        public string LogMessage { get; set; }

        public LogType LogType { get; set; }
    }
}
