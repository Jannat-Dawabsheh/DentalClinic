using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IAppointmentService
    {
        Task<AvilableSlotResponse?> GetAvilableSlots(int Id);
        Task<AppointmentResponse?> BookAppointment(string patientId, int doctorId, BookAppointmentRequest request);
        Task<List<AppointmentListResponseForPatient>?> GetPatientAppointments(string userId, Status? Status);
        Task<List<AppointmentListResponse>?> GetDoctorAppointments(string userId, Status? Status);
        Task<BaseResponse> UpdateAppointmentStatus(UpdateAppointmentRequest request, string userId);
        Task<BaseResponse> DeleteAppointmentByPatient(string userId, int appointmentId);
    }
}
