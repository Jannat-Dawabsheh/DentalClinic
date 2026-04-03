using Azure.Core;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DentalClinic.BLL.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmailSender _emailSender;

        public PatientService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, IEmailSender emailSender)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _emailSender = emailSender;
        }
        public async Task<List<DoctorResposeForPatient>> GetAllDoctorsAsync()
        {
            var doctors = await _patientRepository.GetAllAsync();
            var response = doctors.Adapt<List<DoctorResposeForPatient>>();
            return response;
        }

        public async Task<List<DoctorResposeForPatient>> GetDoctorsBySpecialization(int id, DayOfWeek? day)
        {
            var query = _doctorRepository.Query();
            //var doctors = await _patientRepository.GetDoctorsBySpecialization(id);
            query = query.Where(d => d.Doctor.SpecializationId == id);
            if (day is not null)
            {
                query = query.Where(p => p.DayOfWeek == day);
            }
            var response = query.GroupBy(x => x.Doctor.Id).Select(g => g.First().Doctor).BuildAdapter().AdaptToType<List<DoctorResposeForPatient>>();
            return response;
        }

        public async Task<List<DoctorWorkingDaysRespose>?> GetDoctorWorkingDays(int Id)
        {

            var workingDays = await _doctorRepository.GetWorkingDaysForDoctor(Id);
            if (workingDays is not null)
            {
                var response = workingDays.Adapt<List<DoctorWorkingDaysRespose>>();
                return response;
            }
            else
            {
                return null;
            }

        }

        public DateTime GetNextDayOfWeek(DayOfWeek targetDay)
        {
            var today = DateTime.Today;
            int daysToAdd = ((int)targetDay - (int)today.DayOfWeek + 7) % 7;

            return today.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        }

        public async Task<AvilableSlotResponse?> GetAvilableSlots(int Id)
        {
            var workingDay = await _patientRepository.GetWorkingDayById(Id);
            var date = GetNextDayOfWeek(workingDay.DayOfWeek);
            var start = date.Date.Add(workingDay.StartTime);
            var endOfDay = date.Date.Add(workingDay.EndTime);
            var slots = new List<SlotDTO>();
            while (start < endOfDay)
            {

                var slotEnd = start.Add(TimeSpan.FromMinutes(workingDay.AppointmentDuration));
                if (slotEnd > endOfDay) break;
                slots.Add(new SlotDTO() { StartDateTime = start, EndDateTime = slotEnd });
                start = slotEnd;

            }

            // chek booked

            var bookedAppointments = await _patientRepository.GetBookedAppointments(workingDay.DoctorId, date);

            slots = slots.Where(slot => !bookedAppointments.Any(a => a.StartDateTime < slot.EndDateTime && a.EndDateTime > slot.StartDateTime)).ToList();

            return new AvilableSlotResponse()
            {
                DoctorId = workingDay.DoctorId,
                Date = date,
                Slots = slots
            };
        }


  
        public async Task<AppointmentResponse?>BookAppointment(string userId,int doctorId,BookAppointmentRequest request)
        {

                Doctor doctor = await _doctorRepository.FindByDoctorIdAsync(doctorId);
                Patient patient = await _patientRepository.FindByIdAsync(userId);
               var doctorSchedual = await _patientRepository.isAvailable(doctorId, request.StartDateTime);
               var slots = await GetAvilableSlots(doctorSchedual.Id);
            var slot = new SlotDTO
            {
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
            };
            if (!(slots.Slots.Any(s => s.StartDateTime == request.StartDateTime &&s.EndDateTime == request.EndDateTime)))
            {
                //throw new Exception("Selected time slot is not available");
                return null;
            }

       
            var appointment = request.Adapt<Appointment>();
                appointment.DoctorId = doctorId;
                appointment.PatientId = patient.Id;
                appointment.Status = Status.Pending;
                var response = await _patientRepository.BookAppointment(doctorId,appointment);
                await _emailSender.SendEmailAsync(doctor.User.Email, "New appointment", $"<p>Hello doctor..{doctor.User.UserName}, there is a new appointment request booked by patient : {patient.Id}</p>");

            return response.Adapt<AppointmentResponse>();
            


        }

        public async Task<List<AppointmentListResponseForPatient>?> GetPatientAppointments(string userId, Status? Status)
        {
            var patient = await _patientRepository.FindByIdAsync(userId);
            var Appointments = await _patientRepository.GetAppointmentsForPatient(patient.Id);
            if (Appointments is not null)
            {
                var response = Appointments.Adapt<List<AppointmentListResponseForPatient>>();
                if (Status is not null)
                {
                    response = response.Where(a => a.Status == Status).ToList();
                }
                return response;
            }
            else
            {
                return null;
            }

        }


    }
}
