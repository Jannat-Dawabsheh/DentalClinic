using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Patient
{
    public class VisitDetailsForPatient
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
        public List<string> XRayImages { get; set; } = new();
        public DateTime? NextAppointmentDate { get; set; }
        public List<VisitTreatmentDTO> Treatments { get; set; } = new();
        public List<MedicineResponseDTO> Medicines { get; set; } = new();
    }
}
