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
    public class MedicineRepository: IMedicineRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Medicines.AnyAsync();
        }
        public async Task CreateAsync(List<Medicine> medicines)
        {
            await _context.AddRangeAsync(medicines);
            await _context.SaveChangesAsync();
        }
    }
}
