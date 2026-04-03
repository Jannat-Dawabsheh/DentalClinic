using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Admin
{
    public class DashboardSummary
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalVisits { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal OutstandingPayments { get; set; }
    }
}
