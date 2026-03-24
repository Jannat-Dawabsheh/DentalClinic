using Azure.Core;
using DentalClinic.DAL.Data;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public class DoctorRepository:IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> FindByIdAsync(string id)
        {
            return await _context.Doctors.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task<Doctor?> FindByDoctorIdAsync(int id)
        {
            return await _context.Doctors.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<DoctorSchedules?> FindDayByIdAsync(int id)
        {
            return await _context.DoctorSchedules.Include(c => c.Doctor).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<DoctorSchedules>?>GetWorkingDaysForDoctor(int doctorId)
        {
            try{
               return await _context.DoctorSchedules.Include(d=>d.Doctor).Where(d=>d.DoctorId==doctorId).ToListAsync();
            }
            catch{
                return null;
            }
        }



        public async Task<DoctorSchedules?> AddWorkingDay(DoctorSchedules Request)
        {

            try
            {
                await _context.DoctorSchedules.AddAsync(Request);
                await _context.SaveChangesAsync();
                return Request;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DoctorSchedules>UpdateDayOfWork(DoctorSchedules doctorSchedules)
        {
           
                 _context.DoctorSchedules.Update(doctorSchedules);
                await _context.SaveChangesAsync();
                return doctorSchedules;
         
        }

        public async Task DeleteAsync(DoctorSchedules doctorSchedules)
        {
                _context.DoctorSchedules.Remove(doctorSchedules);
                await _context.SaveChangesAsync();
              
            
        }

        public IQueryable<DoctorSchedules> Query()
        {
            return _context.DoctorSchedules.Include(d=>d.Doctor).ThenInclude(d => d.User).Include(x => x.Doctor.Specialization).AsQueryable();
        }
    }
}
