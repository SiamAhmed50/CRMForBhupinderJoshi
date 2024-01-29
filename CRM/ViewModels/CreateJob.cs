namespace CRM.API.ViewModels
{
    public class CreateJob
    {
        public int ClientId { get; set; }
        public int TaskId { get; set; }

        public CRM.Data.Enums.TaskStatus Status { get; set; }

        public DateTime Started { get; set; }
        public DateTime? Ended { get; set; }

    }
}
