using CRM.Data.Entities;

namespace CRM.API.ViewModels
{
    public class LogsViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public List<LogViewModel> Logs { get; set; }
    }
}
