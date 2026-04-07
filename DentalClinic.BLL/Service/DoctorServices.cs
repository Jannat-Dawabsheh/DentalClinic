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
using System.Numerics;
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
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;

        public DoctorServices(IDoctorRepository doctorRepository, IEmailSender emailSender,
            IPatientRepository patientRepository, IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService)
        {
            _doctorRepository = doctorRepository;
            _emailSender = emailSender;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
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


                var appointments = await _appointmentRepository.GetFutureAppointmentsForDoctorInSpecificDay(workingDay.DoctorId, workingDay.DayOfWeek);
                if (appointments == null || !appointments.Any())
                {
                    return new BaseResponse
                    {
                        Success = true,
                        Message = "Working day updated (no affected appointments)"
                    };
                }

                var invalidAppointments = appointments.Where(a => a.StartDateTime.TimeOfDay < workingDay.StartTime || a.StartDateTime.AddMinutes(workingDay.AppointmentDuration).TimeOfDay > workingDay.EndTime).ToList();

                if (invalidAppointments.Any())
                {




                    foreach (var appointment in invalidAppointments)
                    {
                        appointment.Status = Status.Cancelled;

                    }
                    var success = await _appointmentRepository.CancelRangeOfAppointments(invalidAppointments);

                    if (success)
                    {
                        var emailTasks = invalidAppointments.Select(a => _emailSender.SendEmailAsync(a.Patient.User.Email,
                                          "Canceled appointment",
                                         $"<p>Hello {a.Patient.User.UserName}, your appointment on {a.StartDateTime:yyyy-MM-dd HH:mm} was cancelled due to updated working hours.</p>"
                                            )
                                        );

                        await Task.WhenAll(emailTasks);
                    }

                }
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
                var date = _appointmentService.GetNextDayOfWeek(workingDay.DayOfWeek);
                var appointments= await _appointmentRepository.GetAppointmentsForDoctorInSpecificDay(workingDay.DoctorId, date);
                if(appointments != null && appointments.Any())
                {
                    foreach (var appointment in appointments)
                    {
                        appointment.Status = Status.Cancelled;

                    }
                    var success=await _appointmentRepository.CancelRangeOfAppointments(appointments);

                    if(success)
                    {
                        var emailTasks = appointments.Select(a => _emailSender.SendEmailAsync(a.Patient.User.Email,
                                          "Canceled appointment",
                                           $"<p>Hello {a.Patient.User.UserName}, your appointment booked by doctor {a.Doctor.User.UserName} in day {date.Date} cancelled you can book another one </p>"
                                            )
                                        );

                        await Task.WhenAll(emailTasks);
                    }
                }

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
                    Message = "Failed to delete working day",
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
