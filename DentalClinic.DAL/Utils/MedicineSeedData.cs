using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Utils
{
    public class MedicineSeedData:ISeedData
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineSeedData(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }
        public async Task DataSeed()
        {
            string[] Medicine = ["Paracetamol", "Amoxicillin", "Lacalut"];
            if (!await _medicineRepository.AnyAsync())
            {


                List<Medicine> ListOfMedicems = Medicine.Select(s => new Medicine { Name = s }).ToList();
                await _medicineRepository.CreateAsync(ListOfMedicems);
            }
        }
    }
}
