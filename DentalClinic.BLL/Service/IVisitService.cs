using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
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
        Task<List<VisitResponseForPatient>?> GetAllVisitsForPatient(string userId);
        Task<VisitDetailsForPatient?> GetVisitDetailsForPatient(string userId, int visitId);
        Task<List<VisitResponseForDoctor>?> GetAllVisitsForDoctor(string userId, int? patientId);
        Task<VisitDetailsForDoctor?> GetVisitDetailsForDoctor(int visitId);
        Task<List<VisitResponseForPatient>?> GetPatientVisitsForDoctor(int patientId);
    }
}
