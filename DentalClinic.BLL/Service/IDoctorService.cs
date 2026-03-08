using DentalClinic.DAL.DTO.Request.Admin;
using DentalClinic.DAL.DTO.Response.Admin;
using DentalClinic.DAL.DTO.Response.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IDoctorService
    {
        Task<CreateDoctorResponse> CreateAsync(CreateDoctorRequest Request);
    }
}
