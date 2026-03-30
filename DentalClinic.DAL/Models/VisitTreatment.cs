using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public class VisitTreatment:BaseModel
    {
        public int Id { get; set; }

        public int VisitId { get; set; }
        public Visit Visit { get; set; }
        public string TreatmentName { get; set; }
        public decimal Price { get; set; }
    }
}
