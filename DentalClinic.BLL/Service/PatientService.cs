using Azure.Core;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DentalClinic.BLL.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmailSender _emailSender;

        public PatientService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, IEmailSender emailSender)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _emailSender = emailSender;
        }
        public async Task<List<DoctorResposeForPatient>> GetAllDoctorsAsync()
        {
            var doctors = await _patientRepository.GetAllAsync();
            var response = doctors.Adapt<List<DoctorResposeForPatient>>();
            return response;
        }

        public async Task<List<DoctorResposeForPatient>> GetDoctorsBySpecialization(int id, DayOfWeek? day)
        {
            var query = _doctorRepository.Query();
            //var doctors = await _patientRepository.GetDoctorsBySpecialization(id);
            query = query.Where(d => d.Doctor.SpecializationId == id);
            if (day is not null)
            {
                query = query.Where(p => p.DayOfWeek == day);
            }
            var response = query.GroupBy(x => x.Doctor.Id).Select(g => g.First().Doctor).BuildAdapter().AdaptToType<List<DoctorResposeForPatient>>();
            return response;
        }

        public async Task<List<DoctorWorkingDaysRespose>?> GetDoctorWorkingDays(int Id)
        {

            var workingDays = await _doctorRepository.GetWorkingDaysForDoctor(Id);
            if (workingDays is not null)
            {
                var response = workingDays.Adapt<List<DoctorWorkingDaysRespose>>();
                return response;
            }
            else
            {
                return null;
            }

        }


    }
}
