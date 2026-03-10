using DentalClinic.BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.PL.Areas.Patient
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [HttpGet("Doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var response = await _patientService.GetAllDoctorsAsync();
            return Ok(response);
        }
        [Authorize]
        [HttpGet("Doctors/{id}")]
        public async Task<IActionResult> GetDoctorBySpecialization([FromRoute] int id)
        {
            var response = await _patientService.GetDoctorsBySpecialization(id);
            return Ok(response);
        }
    }
}
