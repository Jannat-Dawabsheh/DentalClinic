using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Admin
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; }
        public string Gender { get; set; }
        public int ExperienceYears { get; set; }
    }
}
