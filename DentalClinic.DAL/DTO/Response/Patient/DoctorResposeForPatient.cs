using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Patient
{
    public class DoctorResposeForPatient
    {

        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Gender { get; set; }
        public int ExperienceYears { get; set; }
    }
}
