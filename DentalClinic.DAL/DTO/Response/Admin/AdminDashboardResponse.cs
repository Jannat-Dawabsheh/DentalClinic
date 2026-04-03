using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Admin
{
    public class AdminDashboardResponse
    {
        public DashboardSummary summary {  get; set; }
        public List<RecentVisitDto> recentVisits { get; set; }
    }
}
