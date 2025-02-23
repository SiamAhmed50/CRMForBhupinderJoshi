namespace CRM.API.ViewModels
{
    public class JobTransactionsViewModel
    {
       
        public int JobId { get; set; }
        public int TransactionNumber { get; set; }    
        public string TransactionDescription { get; set; }    
        public bool TransactionStatus { get; set; } 
        public string TransactionCommand { get; set; }  
        public DateTime Timestamp { get; set; }

        public int Id { get; set; }


    }
}
