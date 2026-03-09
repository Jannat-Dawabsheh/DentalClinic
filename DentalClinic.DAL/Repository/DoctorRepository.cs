using DentalClinic.DAL.Data;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>>GetAllAsync()
        {
            
            var response=await _context.Doctors.Include(u=>u.User).ToListAsync();
            return response;
        }
        public async Task<Doctor> CreateDoctor(Doctor Request)
        {
            await _context.Doctors.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;
        }
        public async Task<Doctor?> FindByIdAsync(int id)
        {
            return await _context.Doctors.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Doctor?> UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
            return doctor;

        }
    }
}
