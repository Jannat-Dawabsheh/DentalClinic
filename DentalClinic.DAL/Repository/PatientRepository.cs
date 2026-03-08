using DentalClinic.DAL.Data;
using DentalClinic.DAL.Models;
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
    }
}
