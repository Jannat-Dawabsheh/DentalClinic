using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IVisitService
    {
        Task<BaseResponse> CheckVisitData(int id, List<int>? medicineIds, DateTime? NextAppointmentDate);
        Task<VisitResponse> CreateVisit(AddVisitRequest request);
    }
}
