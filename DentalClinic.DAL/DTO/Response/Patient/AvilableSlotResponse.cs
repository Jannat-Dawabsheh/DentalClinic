using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Patient
{
    public class AvilableSlotResponse
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public List<SlotDTO> Slots { get; set; }
    }
}
