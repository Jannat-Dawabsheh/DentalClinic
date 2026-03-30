using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface IVisitRepository
    {
        Task<Visit?> Createvisit(Visit Request);
        Task<BaseResponse> CheckVisitData(int id, List<int>? medicineIds, DateTime? NextAppointmentDate);
    }
}
