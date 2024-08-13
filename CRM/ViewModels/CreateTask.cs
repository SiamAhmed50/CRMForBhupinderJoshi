namespace CRM.API.ViewModels
{
    public class CreateTask
    {
        public int ClientId { get; set; }

        public List<string> TaskNames { get; set; }
    }
}
