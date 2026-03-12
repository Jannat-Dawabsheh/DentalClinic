using DentalClinic.DAL.Data;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public class ManageRepository : IManageRepository
    {
        private readonly ApplicationDbContext _context;

        public ManageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>>GetAllAsync()
        {
            
            var response=await _context.Doctors.Include(u=>u.User).Include(u => u.Specialization).ToListAsync();
            return response;
        }
        public async Task<Doctor?> CreateDoctor(Doctor Request)
        {
            
            try
            {
                await _context.Doctors.AddAsync(Request);
                await _context.SaveChangesAsync();
                return Request;
            }
            catch
            {
                return null;
            }
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
