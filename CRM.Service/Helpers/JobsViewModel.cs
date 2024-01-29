using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Helpers
{
    public class JobsViewModel
    {
        public int Id { get; set; }   
        public int? ClientId { get; set; }
        public string? ClientName { get; set; }
        public int? TaskId { get; set; } 
        public string? TaskName { get; set; }
        public string? TaskStatus { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
