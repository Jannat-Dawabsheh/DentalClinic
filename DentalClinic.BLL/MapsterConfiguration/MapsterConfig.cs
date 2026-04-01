using DentalClinic.DAL.DTO.Request.Doctor;
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

            TypeAdapterConfig<Appointment, AppointmentListResponseForPatient>
             .NewConfig()
             .Map(dest => dest.DoctorName, src => src.Doctor.User.FullName);

            TypeAdapterConfig<Patient, PatientDataResponse>
            .NewConfig()
            .Map(dest => dest.FullName, src => src.User.FullName)
            .Map(dest => dest.PhoneNumber, src => src.User.PhoneNumber)
            .Map(dest => dest.Email, src => src.User.Email);

            TypeAdapterConfig<VisitTreatment, VisitTreatmentDTO>.NewConfig();
            TypeAdapterConfig<VisitMedicine, VisitMedicineDTO>
              .NewConfig();

            TypeAdapterConfig<VisitMedicineDTO, VisitMedicine>
           .NewConfig();


            TypeAdapterConfig<VisitMedicine, MedicineResponseDTO>
            .NewConfig()
            .Map(dest => dest.MedicineName, src => src.Medicine.Name)
            .Map(dest => dest.Dosage, src => src.Dosage);

            TypeAdapterConfig<Visit, VisitResponse>
            .NewConfig()
            .Map(dest => dest.XRayImages, source => source.XRayImages.Select(img => $"http://localhost:5238/images/{img.ImageName}").ToList())
             .Map(dest => dest.Treatments, src => src.Treatments)
            .Map(dest => dest.Medicines, src => src.VisitMedicines);

            TypeAdapterConfig<AddVisitRequest, Visit>
            .NewConfig()
            .Map(dest => dest.VisitMedicines, src => src.Medicines);

            TypeAdapterConfig<Visit, VisitDetailsForPatient>
            .NewConfig()
            .Map(dest => dest.XRayImages, source => source.XRayImages.Select(img => $"http://localhost:5238/images/{img.ImageName}").ToList())
             .Map(dest => dest.Treatments, src => src.Treatments)
            .Map(dest => dest.Medicines, src => src.VisitMedicines);

            TypeAdapterConfig<Visit, VisitResponseForDoctor>
         .NewConfig()
          .Map(dest => dest.PatientId, src => src.Appointment.PatientId)
         .Map(dest => dest.PatientName, src => src.Appointment.Patient.User.FullName);

            TypeAdapterConfig<Visit, VisitDetailsForDoctor>
            .NewConfig()
            .Map(dest => dest.XRayImages, source => source.XRayImages.Select(img => $"http://localhost:5238/images/{img.ImageName}").ToList())
             .Map(dest => dest.Treatments, src => src.Treatments)
            .Map(dest => dest.Medicines, src => src.VisitMedicines)
            .Map(dest => dest.PatientId, src => src.Appointment.PatientId)
            .Map(dest => dest.PatientName, src => src.Appointment.Patient.User.FullName);
        }
    }
}
