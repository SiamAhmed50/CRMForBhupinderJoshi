namespace CRM.API.ViewModels
{
    public class JobTransactionsViewModel
    {
       
        public int JobId { get; set; }
        public int TransactionNumber { get; set; }    
        public string TransactionDescription { get; set; }    
        public string TransactionStatus { get; set; } 
        public string TransactionComment { get; set; }  
        public DateTime Timestamp { get; set; }

        public int Id { get; set; }


    }
}
