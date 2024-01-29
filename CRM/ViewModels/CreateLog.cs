namespace CRM.API.ViewModels
{
    public class CreateLog
    {
        public int ClientId { get; set; }
        public int TaskId { get; set; }
        
        public DateTime Timestamp { get; set; }
        public string LogMessage { get; set; }
        public Data.Enums.LogLevel LogLevel { get; set; }

    }
}
