using DentalClinic.DAL.Data;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Patient> CreatePatient(Patient Request)
        {
            await _context.Patients.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {

            var response = await _context.Doctors.Include(u => u.User).Include(u => u.Specialization).ToListAsync();
            return response;
        }

        public async Task<List<Doctor>> GetDoctorsBySpecialization(int id)
        {

            var response = await _context.Doctors.Include(u => u.User).Include(u => u.Specialization).Where(u=>u.SpecializationId==id).ToListAsync();
            return response;
        }
    }
}
