using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Jobs
    {
        [Key]
        public int Id { get; set; }

        public Client? Client { get; set; }

        // Foreign key for Client
        public int ClientId { get; set; }

        public Tasks? Task { get; set; }

        // Foreign key for Client
        public int TaskId { get; set; }

        public TaskStatus? TaskStatus { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
