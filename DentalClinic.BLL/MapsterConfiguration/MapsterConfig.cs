using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.MapsterConfiguration
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Doctor, DoctorResponse>.NewConfig().Map(dest => dest.UserName, source => source.User.UserName)
                .Map(dest => dest.FullName, source => source.User.FullName)
                .Map(dest => dest.Email, source => source.User.Email)
                .Map(dest => dest.PhoneNumber, source => source.User.PhoneNumber)
                .Map(dest => dest.Specialization, source => source.Specialization.Name);

            TypeAdapterConfig<Doctor, DoctorResposeForPatient>.NewConfig()
                .Map(dest => dest.FullName, source => source.User.FullName)
                .Map(dest => dest.Specialization, source => source.Specialization.Name);

            TypeAdapterConfig<DoctorSchedules, DoctorScheduleResponse>.NewConfig()
              .Map(dest => dest.DayOfWeek, source => source.DayOfWeek.ToString());

            TypeAdapterConfig<DoctorSchedules, DoctorResposeForPatient>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Doctor.Id)
            .Map(dest => dest.FullName, src => src.Doctor.User.FullName)
            .Map(dest => dest.Gender, src => src.Doctor.Gender)
            .Map(dest => dest.ExperienceYears, src => src.Doctor.ExperienceYears)
            .Map(dest => dest.Specialization, src => src.Doctor.Specialization.Name);

            TypeAdapterConfig<Appointment, AppointmentResponse>
          .NewConfig()
          .Map(dest => dest.StartDateTime, src => src.StartDateTime)
          .Map(dest => dest.EndDateTime, src => src.EndDateTime);
          


        }
    }
}
