using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public enum PaymentStatus
    {
        Paid,
        UnPaid
    }
    public class Visit: BaseModel
    {
        public int Id {  get; set; }
        public string? Notes {  get; set; }
        public List<XRayImage> XRayImages { get; set; } 
        public DateTime? NextAppointmentDate {  get; set; }
        public List<VisitTreatment> Treatments {  get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public List<VisitMedicine> VisitMedicines { get; set; }

    }
}
