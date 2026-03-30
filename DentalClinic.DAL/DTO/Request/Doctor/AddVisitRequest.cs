using DentalClinic.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Request.Doctor
{
    public class AddVisitRequest
    {
        public string? Notes { get; set; }
        public List<IFormFile> XRayImages { get; set; }
        public DateTime? NextAppointmentDate { get; set; }
        public List<VisitTreatmentDTO> Treatments { get; set; }
        public List<VisitMedicineDTO> Medicines { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int AppointmentId { get; set; }
    }
}
