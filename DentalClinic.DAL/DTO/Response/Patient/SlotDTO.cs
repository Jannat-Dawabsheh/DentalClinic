using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Patient
{
    public class SlotDTO
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
      //  public bool IsAvailable { get; set; }
    }
}
