using DentalClinic.DAL.DTO.Request.Admin;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IManageService
    {
        Task<List<DoctorResponse>> GetAllDoctorsAsync();
        Task<BaseResponse> CreateAsync(CreateDoctorRequest Request);
        Task<BaseResponse> UpdateDoctorAsync(int id, UpdateDoctorRequest request);
        Task<BaseResponse> DeleteDoctorAsync(int id);
        Task<DashboardSummary> GetDashboardSummaryAsync(int? mounth);
        Task<AdminDashboardResponse> GetDashboardResponseAsync(int? mounth);
       


    }
}
