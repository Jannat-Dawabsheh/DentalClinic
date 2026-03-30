using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Request.Doctor
{
    public class VisitTreatmentDTO
    {
        public string TreatmentName { get; set; }
        public decimal Price { get; set; }
    }
}
