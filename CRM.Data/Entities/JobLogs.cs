using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class JobLogs
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int TaskId { get; set; }
        public Tasks? Task { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public List<Logs> Logs { get; set; }

        public JobLogs()
        {
            Logs = [];
        }
    }
}
