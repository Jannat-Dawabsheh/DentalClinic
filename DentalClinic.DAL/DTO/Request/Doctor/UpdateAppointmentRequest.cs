using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Request.Doctor
{
    public class UpdateAppointmentRequest
    {
        public int id {  get; set; }
        public Status status { get; set; }
    }
}
