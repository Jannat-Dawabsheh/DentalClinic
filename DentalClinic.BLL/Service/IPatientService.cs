using DentalClinic.DAL.DTO.Response.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IPatientService
    {
        Task<List<DoctorResposeForPatient>> GetAllDoctorsAsync();
        Task<List<DoctorResposeForPatient>> GetDoctorsBySpecialization(int id);
    }
}
