using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Request.Auth
{
    public class ResetPasswordRequest
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
