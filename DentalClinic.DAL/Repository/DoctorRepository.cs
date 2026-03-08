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
        public async Task<Doctor> CreateDoctor(Doctor Request)
        {
            await _context.Doctors.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;
        }
    }
}
