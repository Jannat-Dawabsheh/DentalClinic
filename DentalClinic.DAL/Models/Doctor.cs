using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Specialization { get; set; }
        public string Gender { get; set; }
        public int ExperienceYears {  get; set; }
    }
}
