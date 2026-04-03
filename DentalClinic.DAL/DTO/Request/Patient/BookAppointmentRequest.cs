using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Request.Patient
{
    public class BookAppointmentRequest
    {
        public DateTime StartDateTime {  get; set; }
        public DateTime EndDateTime { get; set; }

        public bool IsValid(out string error)
        {
            if (StartDateTime >= EndDateTime)
            {
                error = "End time must be after start time";
                return false;
            }

            if (StartDateTime < DateTime.UtcNow)
            {
                error = "Cannot book an appointment in the past";
                return false;
            }

            error = null;
            return true;
        }
    }
}
