using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Doctor
{
    public class UpdateDayOfWorkRequest
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? AppointmentDuration { get; set; }
    }
}
