using Azure.Core;
using DentalClinic.DAL.DTO.Request.Admin;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Auth;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmailSender _emailSender;

        public DoctorService(UserManager<ApplicationUser> userManager,IDoctorRepository doctorRepository, IEmailSender emailSender)
        {
            _userManager = userManager;
            _doctorRepository = doctorRepository;
            _emailSender = emailSender;
        }

        public async Task<List<DoctorResponse>> GetAllDoctorsAsync() 
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var response = doctors.Adapt<List<DoctorResponse>>();
            return response;
        }


        public async Task<BaseResponse> CreateAsync(CreateDoctorRequest Request)
        {
            try
            {


                var user = Request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, Request.Password);

                if (!result.Succeeded)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "User creation failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(user, "Doctor");
                Doctor doctor = Request.Adapt<Doctor>();
               doctor.UserId=user.Id;

                await _doctorRepository.CreateDoctor(doctor);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"http://localhost:5238/api/auth/Accounts/ConfirmEmail?token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "welcome", $"<h1>welcome doctor..{user.UserName}, your password is {Request.Password}</h1>" + $"<a href='{emailUrl}'>confirm email</a>");
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


        public async Task<BaseResponse> UpdateDoctorAsync(int id, UpdateDoctorRequest request)
        {
            try
            {
                var doctor = await _doctorRepository.FindByIdAsync(id);
                if (doctor is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "doctor Not Found"
                    };
                }


                if (request.Email != null)
                {
                    doctor.User.Email = request.Email;
                }

                if (request.UserName != null)
                {
                    doctor.User.UserName = request.UserName;
                }


                if (request.FullName != null)
                {
                    doctor.User.FullName = request.FullName;
                }

    

                if (request.PhoneNumber != null)
                {
                    doctor.User.PhoneNumber = request.PhoneNumber;
                }

                if (request.Specialization != null)
                {
                    doctor.Specialization = request.Specialization;
                }

                if (request.Gender != null)
                {
                    doctor.Gender = request.Gender;
                }

                if (request.ExperienceYears != null)
                {
                    doctor.ExperienceYears =(int) request.ExperienceYears;
                }



             
            
                await _doctorRepository.UpdateAsync(doctor);
                return new BaseResponse
                {
                    Success = true,
                    Message = "doctor Updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't update doctor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<BaseResponse> DeleteDoctorAsync(int id)

        {
            try
            {
                var doctor = await _doctorRepository.FindByIdAsync(id);
                if (doctor is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Doctor Not Found"
                    };
                }
               
                await _userManager.DeleteAsync(doctor.User);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Doctor Deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't Delete Doctor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }    
}
