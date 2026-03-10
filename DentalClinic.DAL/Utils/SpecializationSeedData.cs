using DentalClinic.DAL.Migrations;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Utils
{
    public class SpecializationSeedData:ISeedData
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationSeedData(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }
        public async Task DataSeed()
        {
            string[] Specializations = ["Dental implants", "Cosmetic Dentistry", "Dental Surgery", "Orthondontics", "Dental public health specialist"];
            if (!await _specializationRepository.AnyAsync())
            {
               

                List<Specialization> ListOfSpecialization = Specializations.Select(s => new Specialization { Name = s }).ToList();
                await _specializationRepository.CreateAsync(ListOfSpecialization);
            }
        }
    }
}
