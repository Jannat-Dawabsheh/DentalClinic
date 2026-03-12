using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IDoctorServices
    {
        Task<List<DoctorScheduleResponse>?> GetDoctorWorkingDays(string userId);
        Task<BaseResponse> AddWorkingDay(string userId, AddDayRequest Request);
        
       Task<BaseResponse> UpdateworkingDay(int id, UpdateDayOfWorkRequest request);
        Task<BaseResponse> DeleteWorkingDayAsync(int id);
    }
}
