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
    }
}
