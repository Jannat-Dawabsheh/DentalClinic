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
    }
}
