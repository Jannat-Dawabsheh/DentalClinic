using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface IManageRepository
    {
        Task<Doctor> CreateDoctor(Doctor Request);
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor?> FindByIdAsync(int id);
        Task<Doctor?> UpdateAsync(Doctor doctor);
    }
}
