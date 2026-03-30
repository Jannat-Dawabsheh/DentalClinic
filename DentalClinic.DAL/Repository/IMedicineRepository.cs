using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface IMedicineRepository
    {
        Task<bool> AnyAsync();
        Task CreateAsync(List<Medicine> medicines);
    }
}
