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
    public class JobLogs
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }
        [ForeignKey("TaskId")]
        public Tasks? Task { get; set; } 
        // Foreign key for Client
        public int ClientId { get; set; }
     
        public int TaskId { get; set; }

        public List<Logs> Logs { get; set; }





    }
}
