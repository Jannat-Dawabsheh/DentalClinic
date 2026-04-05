using Azure.Core;
using DentalClinic.DAL.Data;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<DoctorSchedules?> GetWorkingDayById(int Id)
        {
            try
            {
                return await _context.DoctorSchedules.FirstOrDefaultAsync(d => d.Id == Id);
            }
            catch
            {
                return null;
            }
        }




        public async Task<Patient?> FindByIdAsync(string id)
        {
            return await _context.Patients.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == id);
        }



        public async Task<Patient?> FindByPatientIdAsync(int id)
        {
            return await _context.Patients.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }



    }
}
