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

        public string Name { get; set; }

        public ClientTask ClientTask { get; set; }

        // Foreign key for Client
        public int ClientTaskId { get; set; }

        // Custom logic to generate unique 4-digit IDs
        public void GenerateUnique4DigitId()
        {
            Random random = new Random();
            HashSet<int> existingIds = new HashSet<int>();

            // Ensure that the generated ID is unique
            do
            {
                Id = random.Next(1000, 10000);
            } while (!existingIds.Add(Id));
        }
    }


}
