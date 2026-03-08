using DentalClinic.DAL.DTO.Request.Admin;
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
        public async Task<CreateDoctorResponse> CreateAsync(CreateDoctorRequest Request)
        {
            try
            {


                var user = Request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, Request.Password);

                if (!result.Succeeded)
                {
                    return new CreateDoctorResponse()
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
                return new CreateDoctorResponse()
                {
                    Success = true,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {
                return new CreateDoctorResponse()
                {
                    Success = false,
                    Message = "Exception error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
