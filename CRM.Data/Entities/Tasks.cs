using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }

        public string Name { get; set; }

        public ClientTask? ClientTask { get; set; }

        // Foreign key for Client
        public int ClientTaskId { get; set; }

       
    }


}
