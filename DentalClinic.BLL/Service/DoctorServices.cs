using Azure.Core;
using DentalClinic.DAL.DTO.Request.Admin;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class DoctorServices:IDoctorServices
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorServices(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
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

    }
}
