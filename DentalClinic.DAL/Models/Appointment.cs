using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public enum Status
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }
    public class Appointment
    {
        public int Id { get; set; } 
        public int PatientId {  get; set; }
        public Patient Patient { get; set; }
        public int DoctorId {  get; set; }
        public Doctor Doctor { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Status Status { get; set; }

    }
}
