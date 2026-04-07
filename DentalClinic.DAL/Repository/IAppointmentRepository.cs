using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetBookedAppointments(int doctorId, DateTime date);
        Task<Appointment> BookAppointment(int doctorId, Appointment Request);
        Task<List<Appointment>?> GetAppointmentsForPatient(int patientId);
        Task<DoctorSchedules> isAvailable(int doctorId, DateTime startDate);
        Task<bool> hasConflict(Patient patient, BookAppointmentRequest request);
        Task<List<Appointment>?> GetAppointmentsForDoctor(int doctorId);

        Task<Appointment> UpdateAppointmentStatus(Appointment appointment);
        Task<Appointment?> FindAppointmentByIdAsync(int id);
        Task DeleteAppointmentByPatient(Appointment appointment);
        Task<List<Appointment>?> GetAppointmentsForDoctorInSpecificDay(int doctorId, DateTime date);
        Task<bool> CancelRangeOfAppointments(List<Appointment> appointments);
        Task<List<Appointment>?> GetFutureAppointmentsForDoctorInSpecificDay(int doctorId, DayOfWeek day);
    }
}
