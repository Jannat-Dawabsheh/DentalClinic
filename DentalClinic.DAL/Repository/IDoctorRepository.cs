using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface IDoctorRepository
    {
        Task<Doctor?> FindByIdAsync(string id);
        Task<DoctorSchedules?> FindDayByIdAsync(int id);
        Task<Doctor?> FindByDoctorIdAsync(int id);
        Task<List<DoctorSchedules>?> GetWorkingDaysForDoctor(int id);

        Task<DoctorSchedules?> AddWorkingDay(DoctorSchedules Request);
        Task<DoctorSchedules> UpdateDayOfWork(DoctorSchedules doctorSchedules);
        Task DeleteAsync(DoctorSchedules doctorSchedules);
        IQueryable<DoctorSchedules> Query();

    }
    
}
