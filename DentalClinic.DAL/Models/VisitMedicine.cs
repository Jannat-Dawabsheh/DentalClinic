using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public class VisitMedicine:BaseModel
    {
        public int Id { get; set; }

        public int VisitId { get; set; }
        public Visit Visit { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public string Dosage { get; set; }
    }
}
