using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface IPatientRepository
    {
        Task<Patient> CreatePatient(Patient Request);
        Task<List<Doctor>> GetAllAsync();
        Task<List<Doctor>> GetDoctorsBySpecialization(int id);
        Task<DoctorSchedules?> GetWorkingDayById(int Id);

        Task<Patient?> FindByIdAsync(string id);

        Task<Patient?> FindByPatientIdAsync(int id);

    }
}
