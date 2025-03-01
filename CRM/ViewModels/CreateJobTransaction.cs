namespace CRM.API.ViewModels
{
    public class CreateJobTransaction
    {
        public int JobId { get; set; }
        public int TransactionNumber { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionCommand { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
