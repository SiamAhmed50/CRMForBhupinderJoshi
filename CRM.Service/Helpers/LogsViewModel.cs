using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Helpers
{
    public class LogsViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }

        public List<Logs> logs { get; set; }
    }
}
