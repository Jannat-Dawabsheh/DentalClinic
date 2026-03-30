using Azure.Core;
using DentalClinic.DAL.DTO.Request.Admin;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class DoctorServices:IDoctorServices
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmailSender _emailSender;
        private readonly IPatientRepository _patientRepository;

        public DoctorServices(IDoctorRepository doctorRepository, IEmailSender emailSender, IPatientRepository patientRepository)
        {
            _doctorRepository = doctorRepository;
            _emailSender = emailSender;
            _patientRepository = patientRepository;
        }

        public async Task<List<DoctorScheduleResponse>?>GetDoctorWorkingDays(string userId)
        {
            var doctor = await _doctorRepository.FindByIdAsync(userId);
            var workingDays= await _doctorRepository.GetWorkingDaysForDoctor(doctor.Id);
            if (workingDays is not null)
            {
                var response = workingDays.Adapt<List<DoctorScheduleResponse>>();
                return response;
            }
            else
            {
                return null;
            }

        }
        public async Task<BaseResponse> AddWorkingDay(string userId,AddDayRequest Request)
        {
            try
            {
                if (Request.EndTime <= Request.StartTime)
                {
                     return new BaseResponse()
                    {
                        Success = false,
                        Message = "EndTime must be greater than StartTime",

                    }; 
                     
                }
                var doctor = await _doctorRepository.FindByIdAsync(userId);

                if (doctor == null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Doctor not found"
                    };
                }


                var WorkingDay = Request.Adapt<DoctorSchedules>();
                WorkingDay.DoctorId = doctor.Id;
                var result = await _doctorRepository.AddWorkingDay(WorkingDay);

               
                if (result is null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Can't add working day",
                       
                    };
                }

                return new BaseResponse()
                {
                    Success = true,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Exception error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> UpdateworkingDay(int id, UpdateDayOfWorkRequest request)
        {
            try
            {
                var workingDay = await _doctorRepository.FindDayByIdAsync(id);
                if (workingDay is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "workingDay Not Found"
                    };
                }


                if (request.StartTime != null)
                {
                    workingDay.StartTime = (TimeSpan)request.StartTime;
                }

                if (request.EndTime != null)
                {
                    workingDay.EndTime = (TimeSpan)request.EndTime;
                }


                if (request.AppointmentDuration != null)
                {
                    workingDay.AppointmentDuration =(int) request.AppointmentDuration;
                }


                await _doctorRepository.UpdateDayOfWork(workingDay);
                return new BaseResponse
                {
                    Success = true,
                    Message = "working Day Updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't update working Day",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> DeleteWorkingDayAsync(int id)

        {
            try
            {
                var workingDay = await _doctorRepository.FindDayByIdAsync(id);
                if (workingDay is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "workingDay Not Found"
                    };
                }

                await _doctorRepository.DeleteAsync(workingDay);
                return new BaseResponse
                {
                    Success = true,
                    Message = "working Day Deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't workingDay Doctor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<List<AppointmentListResponse>?> GetDoctorAppointments(string userId,Status? Status)
        {
            var doctor = await _doctorRepository.FindByIdAsync(userId);
            var Appointments = await _doctorRepository.GetAppointmentsForDoctor(doctor.Id);
            if (Appointments is not null)
            {
                var response = Appointments.Adapt<List<AppointmentListResponse>>();
                if(Status is not null)
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



        public async Task<BaseResponse> UpdateAppointmentStatus(UpdateAppointmentRequest request,string userId)
        {
        
      
            try
            {
                var Appointment = await _doctorRepository.FindAppointmentByIdAsync(request.id);
                if (Appointment is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Appointment Not Found"
                    };
                }
                if(Appointment.Doctor.UserId != userId)
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
                


                await _doctorRepository.UpdateAppointmentStatus(Appointment);

                if(Appointment.Status == Status.Confirmed)
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


        public async Task<BaseResponse> UpdatePatientData(UpdatePatientDataRequest request,int id)
        {


            try
            {
                var patient = await _patientRepository.FindByPatientIdAsync(id);
                if (patient is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "patient Not Found"
                    };
                }


                if (request.DateOfBirth != null)
                {
                    patient.DateOfBirth = request.DateOfBirth;
                }

                if (request.Gender != null)
                {
                    patient.Gender = request.Gender;
                }
                if (request.Allergies != null)
                {
                    patient.Allergies = request.Allergies;
                }
                if (request.Diseases != null)
                {
                    patient.Diseases = request.Diseases;
                }


                await _doctorRepository.UpdatePatientData(patient);

               
                return new BaseResponse
                {
                    Success = true,
                    Message = "patient data Updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't update patient data",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<PatientDataResponse?> GetPatientByIdOrName(int? Id, String? Name)
        {
            Patient patient;
            if (Id is not null)
            {
                patient = await _doctorRepository.GetPatientByNameOrId(Id, null);
                if (patient is not null)
                {
                    var response = patient.Adapt<PatientDataResponse>();
                    return response;
                }
                else
                {
                    return null;
                }
            }
            else if (Name is not null)
            {
                patient = await _doctorRepository.GetPatientByNameOrId(null, Name);

                if (patient is not null)
                {
                    var response = patient.Adapt<PatientDataResponse>();
                    return response;
                }
                else
                {
                    return null;
                }
            }
            return null;

        }


    }
}
