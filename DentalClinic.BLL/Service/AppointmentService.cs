using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class AppointmentService: IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmailSender _emailSender;

        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IEmailSender emailSender)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _emailSender = emailSender;
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

            var bookedAppointments = await _appointmentRepository.GetBookedAppointments(workingDay.DoctorId, date);

            slots = slots.Where(slot => !bookedAppointments.Any(a => a.StartDateTime < slot.EndDateTime && a.EndDateTime > slot.StartDateTime)).ToList();

            return new AvilableSlotResponse()
            {
                DoctorId = workingDay.DoctorId,
                Date = date,
                Slots = slots
            };
        }



        public async Task<AppointmentResponse?> BookAppointment(string userId, int doctorId, BookAppointmentRequest request)
        {

            Doctor doctor = await _doctorRepository.FindByDoctorIdAsync(doctorId);
            Patient patient = await _patientRepository.FindByIdAsync(userId);
            var doctorSchedual = await _appointmentRepository.isAvailable(doctorId, request.StartDateTime);
            var slots = await GetAvilableSlots(doctorSchedual.Id);
      
            if (!(slots.Slots.Any(s => s.StartDateTime == request.StartDateTime && s.EndDateTime == request.EndDateTime)))
            {
                //throw new Exception("Selected time slot is not available");
                return null;
            }

            var hasConflict = await _appointmentRepository.hasConflict(patient, request);

            if (hasConflict)
                throw new Exception("Patient already has an overlapping appointment");


            var appointment = request.Adapt<Appointment>();
            appointment.DoctorId = doctorId;
            appointment.PatientId = patient.Id;
            appointment.Status = Status.Pending;
            var response = await _appointmentRepository.BookAppointment(doctorId, appointment);
            await _emailSender.SendEmailAsync(doctor.User.Email, "New appointment", $"<p>Hello doctor..{doctor.User.UserName}, there is a new appointment request booked by patient : {patient.Id}</p>");

            return response.Adapt<AppointmentResponse>();



        }

        public async Task<List<AppointmentListResponseForPatient>?> GetPatientAppointments(string userId, Status? Status)
        {
            var patient = await _patientRepository.FindByIdAsync(userId);
            var Appointments = await _appointmentRepository.GetAppointmentsForPatient(patient.Id);
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

        public async Task<List<AppointmentListResponse>?> GetDoctorAppointments(string userId, Status? Status)
        {
            var doctor = await _doctorRepository.FindByIdAsync(userId);
            var Appointments = await _appointmentRepository.GetAppointmentsForDoctor(doctor.Id);
            if (Appointments is not null)
            {
                var response = Appointments.Adapt<List<AppointmentListResponse>>();
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



        public async Task<BaseResponse> UpdateAppointmentStatus(UpdateAppointmentRequest request, string userId)
        {


            try
            {
                var Appointment = await _appointmentRepository.FindAppointmentByIdAsync(request.id);
                if (Appointment is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Appointment Not Found"
                    };
                }
                if (Appointment.Doctor.UserId != userId)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "You can't change this appointment status"
                    };
                }
                if (Appointment.Status == Status.Completed)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Can't change this appointment status"
                    };
                }

                Appointment.Status = request.status;



                await _appointmentRepository.UpdateAppointmentStatus(Appointment);

                if (Appointment.Status == Status.Confirmed)
                {
                    await _emailSender.SendEmailAsync(Appointment.Patient.User.Email, "Appointment Confirmed", $"<p>Hello ..{Appointment.Patient.User.UserName}, Your appointment was confirmed by doctor : {Appointment.Doctor.User.UserName}</p>");

                }
                else if (Appointment.Status == Status.Cancelled)
                {
                    await _emailSender.SendEmailAsync(Appointment.Patient.User.Email, "Appointment Cancelled", $"<p>Hello ..{Appointment.Patient.User.UserName}, Your appointment was cancelled by doctor : {Appointment.Doctor.User.UserName}</p>");

                }
                return new BaseResponse
                {
                    Success = true,
                    Message = "Appointment Updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't update Appointment",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse>DeleteAppointmentByPatient(string userId,int appointmentId)
        {
            var patient=await _patientRepository.FindByIdAsync(userId);
            if(patient is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Patient not found"
                };
            }
            var appointment=await _appointmentRepository.FindAppointmentByIdAsync(appointmentId);

            if (appointment is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "appointment not found"
                };
            }

            try
            {
                if (appointment.Patient.UserId != userId)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "you can't delete this appointment"
                    };
                }

                if (appointment.Status != Status.Pending)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Only pending appointments can be cancelled by the patient"
                    };
                }

                appointment.Status = Status.Cancelled;

                await _appointmentRepository.UpdateAppointmentStatus(appointment);
                await _emailSender.SendEmailAsync(appointment.Doctor.User.Email, "Delet appointment", $"<p>Hello doctor..{appointment.Doctor.User.UserName}, Patient {patient.User.UserName} cancelled the appointment request at {appointment.StartDateTime} </p>");


                return new BaseResponse
                {
                    Success = true,
                    Message = "Appointment deleted successfully"
                };

            }
            catch (Exception ex) {
                
                    return new BaseResponse
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
        }

       

    }
}
