using DentalClinic.BLL.Service;
using DentalClinic.DAL.DTO.Request.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> GetDoctorBySpecialization([FromRoute] int id, [FromQuery]int? WorkingDay)
        {
            var response = await _patientService.GetDoctorsBySpecialization(id, (DayOfWeek?)WorkingDay);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("Doctors/workingDays/{id}")]
        public async Task<IActionResult> GetDoctorWorkingDays([FromRoute] int id)
        {
            var response = await _patientService.GetDoctorWorkingDays(id);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("Doctors/workingDaySlots/{id}")]
        public async Task<IActionResult> GetWorkingDaySlot([FromRoute] int id)
        {
            var response = await _patientService.GetAvilableSlots(id);
            return Ok(response);
        }


        [Authorize]
        [HttpPost("BookAppointment/{doctorId}")]
        public async Task<IActionResult> BookAppointment([FromRoute] int doctorId, [FromBody]BookAppointmentRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _patientService.BookAppointment(userId, doctorId, request);
                return Ok(response);
            }
            catch (Exception ex) { 
              return BadRequest("This appointment has already booked");
            }
        }
    }
}
