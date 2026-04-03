using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Admin
{
    public class RecentVisitDto
    {
        public int VisitId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime VisitDate { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
