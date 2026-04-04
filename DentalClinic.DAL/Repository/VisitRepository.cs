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

        public async Task<List<Visit>?>GetAllVisitsForPatient(int PatientId)
        {
            return await _context.Visits
                .Include(v => v.XRayImages)
                .Include(v => v.Treatments)
                .Include(v => v.VisitMedicines)
                .ThenInclude(vm => vm.Medicine)
                .Where(v => v.Appointment.PatientId == PatientId).ToListAsync();
        }

        public async Task<Visit?> GetVisitDetailsForPatient(int PatientId,int visitId)
        {
            return await _context.Visits
                .Include(v => v.XRayImages)
                .Include(v => v.Treatments)
                .Include(v => v.VisitMedicines)
                .ThenInclude(vm => vm.Medicine)
                .FirstOrDefaultAsync(v => v.Appointment.PatientId == PatientId && v.Id==visitId);
        }

        public async Task<List<Visit>?> GetAllVisitsForDoctor(int DoctorId,int? patientId)
        {
            if(patientId is not null)
            {
                return await _context.Visits
                 .Include(v => v.XRayImages)
                 .Include(v => v.Treatments)
                 .Include(v => v.VisitMedicines)
                 .ThenInclude(vm => vm.Medicine)
                 .Include(v => v.Appointment).ThenInclude(a => a.Patient).ThenInclude(u => u.User)

                 .Where(v => v.Appointment.DoctorId == DoctorId &&  v.Appointment.PatientId==patientId).ToListAsync();
            }
            return await _context.Visits
                .Include(v => v.XRayImages)
                .Include(v => v.Treatments)
                .Include(v => v.VisitMedicines)
                .ThenInclude(vm => vm.Medicine)
                .Include(v => v.Appointment).ThenInclude(a => a.Patient).ThenInclude(u => u.User)
                
                .Where(v => v.Appointment.DoctorId == DoctorId).ToListAsync();
        }

        public async Task<Visit?> GetVisitDetailsForDoctr( int visitId)
        {
            return await _context.Visits
                .Include(v => v.XRayImages)
                .Include(v => v.Treatments)
                .Include(v => v.VisitMedicines)
                .ThenInclude(vm => vm.Medicine)
                 .Include(v => v.Appointment).ThenInclude(a => a.Patient).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(v =>  v.Id == visitId);
        }

        public async Task<Appointment?>GetAppointmentById(int Id)
        {
            return await _context.Appointments.Include(c => c.Doctor).Include(c=>c.Patient).FirstOrDefaultAsync(c => c.Id == Id);
        }

    }
}
