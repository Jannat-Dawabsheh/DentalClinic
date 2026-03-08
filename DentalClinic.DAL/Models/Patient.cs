using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User {  get; set; }
        public List<string>? Diseases {  get; set; }
        public List<string>? Allergies { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth {  get; set; }

    }
}
