using Azure.Core;
using DentalClinic.DAL.Data;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public class VisitRepository: IVisitRepository
    {
        private readonly ApplicationDbContext _context;

        public VisitRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse> CheckVisitData(int id, List<int>? medicineIds,DateTime? NextAppointmentDate)
        {

              if (!await _context.Appointments.Where(v => v.Id == id).AnyAsync())
                return new BaseResponse
                {
                    Success = false,
                    Message = "This appointment not exist"
                }; 

            else if(await _context.Visits.Where(v => v.AppointmentId == id).AnyAsync())
            return new BaseResponse
            {
                Success = false,
                Message= "This visit is already added"
            } ;

            if (medicineIds != null && medicineIds.Any())
            {
                var existingMedicineIds = await _context.Medicines
                    .Where(m => medicineIds.Contains(m.Id))
                    .Select(m => m.Id)
                    .ToListAsync();

                var invalidIds = medicineIds.Except(existingMedicineIds).ToList();

                if (invalidIds.Any())

                    return new BaseResponse
                    {
                        Success = false,
                        Message = "This medicine not exist"
                    };
            }
             if(NextAppointmentDate.HasValue &&  NextAppointmentDate < DateTime.UtcNow)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid date"
                };

            return new BaseResponse
                {
                    Success = true,
                };

        }
        public async Task<Visit?> Createvisit(Visit Request)
        {

            try
            {
                await _context.Visits.AddAsync(Request);
                await _context.SaveChangesAsync();
                return await _context.Visits
                  .Include(v => v.XRayImages)
                  .Include(v => v.Treatments)
                  .Include(v => v.VisitMedicines)
                  .ThenInclude(vm => vm.Medicine) 
                  .FirstOrDefaultAsync(v => v.Id == Request.Id);
            }
            catch
            {
                return null;
            }
        }


    }
}
