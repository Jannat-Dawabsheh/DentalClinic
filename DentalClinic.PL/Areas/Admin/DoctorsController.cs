using DentalClinic.BLL.Service;
using DentalClinic.DAL.DTO.Request.Admin;
using DentalClinic.DAL.DTO.Response.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.PL.Areas.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpPost("")]
        public async Task<IActionResult>CreateDoctor(CreateDoctorRequest request)
        {
            var response=await _doctorService.CreateAsync(request);
            return  Ok(response);
        }
    }
}
