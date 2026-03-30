using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Request.Doctor
{
    public class VisitMedicineDTO

    {
        public int MedicineId { get; set; }
        //public int MedicineName { get; set; }
        public string Dosage { get; set; }
    }
}
