using DentalClinic.DAL.Data;
using DentalClinic.DAL.DTO.Request;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public class AppointmentRepository: IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Appointment>> GetBookedAppointments(int doctorId, DateTime date)
        {
            return await _context.Appointments.Where(a => a.DoctorId == doctorId && a.StartDateTime.Date == date.Date && (a.Status==Status.Confirmed || a.Status == Status.Pending)).ToListAsync();

        }

        public async Task<DoctorSchedules> isAvailable(int doctorId, DateTime startDate)
        {
            var dayOfWeek = startDate.DayOfWeek;
            return await _context.DoctorSchedules.FirstOrDefaultAsync(d => d.DoctorId == doctorId && d.DayOfWeek == dayOfWeek);

        }
        public async Task<Appointment> BookAppointment(int doctorId, Appointment Request)
        {
            var exists = await _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.StartDateTime < Request.EndDateTime &&
            a.EndDateTime > Request.StartDateTime &&
            (a.Status == Status.Pending || a.Status == Status.Confirmed)
             );

            if (exists)
            {
                throw new BadRequestException("Slot already booked");
            }
            await _context.Appointments.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;

        }
        public async Task<List<Appointment>?> GetAppointmentsForPatient(int patientId)
        {
            try
            {
                return await _context.Appointments.Include(d => d.Patient).Include(d => d.Doctor).ThenInclude(u => u.User).Where(d => d.PatientId == patientId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> hasConflict(Patient patient, BookAppointmentRequest request)

        {
            return await _context.Appointments.AnyAsync(a => a.PatientId == patient.Id && a.StartDateTime < request.EndDateTime && a.EndDateTime > request.StartDateTime && (a.Status == Status.Pending || a.Status == Status.Confirmed));
        }


        public async Task<List<Appointment>?> GetAppointmentsForDoctor(int doctorId)
        {
            try
            {
                return await _context.Appointments.Include(d => d.Doctor).Where(d => d.DoctorId == doctorId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Appointment> UpdateAppointmentStatus(Appointment appointment)
        {

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;

        }

        public async Task<Appointment?> FindAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.Include(d => d.Doctor).ThenInclude(u => u.User).Include(p => p.Patient).ThenInclude(u => u.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteAppointmentByPatient(Appointment appointment)
        {
             _context.Appointments.Remove(appointment);   
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>?> GetAppointmentsForDoctorInSpecificDay(int doctorId,DateTime date)
        {
            try
            {
                return await _context.Appointments.Include(d => d.Doctor).ThenInclude(p => p.User).Include(p=>p.Patient).ThenInclude(p=>p.User).Where(a=>a.StartDateTime.Date==date.Date && a.DoctorId == doctorId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<bool>CancelRangeOfAppointments(List<Appointment> appointments)
        {
            try
            {
                _context.Appointments.UpdateRange(appointments);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
            return false;
            }
        }

        public async Task<List<Appointment>?> GetFutureAppointmentsForDoctorInSpecificDay(int doctorId, DayOfWeek day)
        {
            try
            {
                var today = DateTime.Today;
                var endDate = today.AddDays(8);

                var appointments = await _context.Appointments
                    .Include(d => d.Doctor).ThenInclude(p => p.User)
                    .Include(p => p.Patient).ThenInclude(p => p.User)
                    .Where(a =>
                        a.DoctorId == doctorId &&
                        a.StartDateTime >= today &&
                        a.StartDateTime < endDate
                    )
                    .ToListAsync();

                return appointments
                    .Where(a => a.StartDateTime.DayOfWeek == day)
                    .ToList();
           
            }
            catch
            {
                return null;
            }
        }

    }
}
