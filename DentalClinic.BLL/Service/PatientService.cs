using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class PatientService: IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<List<DoctorResposeForPatient>> GetAllDoctorsAsync()
        {
            var doctors = await _patientRepository.GetAllAsync();
            var response = doctors.Adapt<List<DoctorResposeForPatient>>();
            return response;
        }

        public async Task<List<DoctorResposeForPatient>> GetDoctorsBySpecialization(int id)
        {
            var doctors = await _patientRepository.GetDoctorsBySpecialization(id);
            var response = doctors.Adapt<List<DoctorResposeForPatient>>();
            return response;
        }
    }
}
