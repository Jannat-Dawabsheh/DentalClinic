using DentalClinic.BLL.Service;
using DentalClinic.DAL.Repository;
using DentalClinic.DAL.Utils;
using kashop.bll.Service;
using kashop.pl.Middleware;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DentalClinic.PL
{


    public static class AppConfiguration
    {
        public static void Config(IServiceCollection Services)
        {

            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddScoped<IPatientRepository, PatientRepository>();
            Services.AddScoped<IEmailSender, EmailSender>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IDoctorService, DoctorService>();
            Services.AddScoped<IDoctorRepository, DoctorRepository>();
            Services.AddExceptionHandler<GlobalExceptionHandler>();
            Services.AddProblemDetails();

        }
    }
}
