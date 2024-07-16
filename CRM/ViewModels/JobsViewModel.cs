namespace CRM.API.ViewModels
{
    public class JobsViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
