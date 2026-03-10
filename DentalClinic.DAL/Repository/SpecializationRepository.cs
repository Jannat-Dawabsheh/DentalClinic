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
    public class SpecializationRepository:ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecializationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AnyAsync()
        {
           return await  _context.Specializations.AnyAsync();
        }

        public async Task CreateAsync(List<Specialization> specialization)
        {
           await _context.AddRangeAsync(specialization);
           await _context.SaveChangesAsync();
        }



    }
}
