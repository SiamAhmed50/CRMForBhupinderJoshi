using CRM.Data.Enums;

namespace CRM.API.ViewModels
{
    public class UpdateJob
    {
        public int JobId { get; set; }

        public DateTime Ended { get; set; }

        public JobTaskStatus Status { get; set; }
    }
}
