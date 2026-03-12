using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Doctor
{
    public class DoctorScheduleResponse
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DayOfWeek { get; set; }
        public int AppointmentDuration { get; set; }
    }
}
