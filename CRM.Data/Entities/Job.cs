using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int TasksId { get; set; }
        public Tasks? Tasks { get; set; }

        public TaskStatus Status { get; set; }

        public DateTime Started { get; set; }

        public DateTime? Ended { get; set; }
    }
}
