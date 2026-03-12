using DentalClinic.BLL.Service;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using DentalClinic.DAL.Utils;
using kashop.bll.Service;
using kashop.pl.Middleware;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.PL
{


    public static class AppConfiguration
    {
        public static void Config(IServiceCollection Services)
        {

            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddScoped<ISeedData, SpecializationSeedData>();
            Services.AddScoped<IPatientService, PatientService>();
            Services.AddScoped<IPatientRepository, PatientRepository>();
            Services.AddScoped<IEmailSender, EmailSender>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IManageService, ManageService>();
            Services.AddScoped<IManageRepository, ManageRepository>();
            Services.AddScoped<IManageRepository, ManageRepository>();
            Services.AddScoped<IDoctorRepository, DoctorRepository>();
            Services.AddScoped<IDoctorServices, DoctorServices>();
            Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            Services.AddExceptionHandler<GlobalExceptionHandler>();
            Services.AddProblemDetails();

        }
    }
}

