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
        Task<List<Visit>?> GetAllVisitsForPatient(int PatientId);
        Task<Visit?> GetVisitDetailsForPatient(int PatientId, int visitId);
        Task<List<Visit>?> GetAllVisitsForDoctor(int DoctorId, int? patientId);
        Task<Visit?> GetVisitDetailsForDoctr(int visitId);
        Task<Appointment?> GetAppointmentById(int Id);
    }
}
