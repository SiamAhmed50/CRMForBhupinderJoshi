using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Jobs
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ClientId")]
        public Client? Client { get; set; } 
        // Foreign key for Client
        public int ClientId { get; set; }
        [ForeignKey("TaskId")]
        public Tasks? Tasks { get; set; }
        public int TaskId { get; set; }

        public CRM.Data.Enums.TaskStatus? TaskStatus { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

         

    }
}
