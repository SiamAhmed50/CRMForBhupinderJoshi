using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class JobTransactions
    {
        [Key]
        public int Id { get; set; }
        public int JobId { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
