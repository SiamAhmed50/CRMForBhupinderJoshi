namespace CRM.API.ViewModels
{
    public class CreateLog
    {
        public int JobId { get; set; }

        public DateTime Timestamp { get; set; }

        public string LogMessage { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}
