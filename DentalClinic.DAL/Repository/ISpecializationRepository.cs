using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Repository
{
    public interface ISpecializationRepository
    {
        Task<bool> AnyAsync();
        Task CreateAsync(List<Specialization> specialization);
    }
}
