using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Admin
{
    public class CreateDoctorResponse:BaseResponse
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; }
    }
}
