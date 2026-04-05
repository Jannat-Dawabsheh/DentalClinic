using DentalClinic.BLL.Service;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DentalClinic.PL.Areas.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Doctor")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorServices _doctorServices;
        private readonly IAppointmentService _appointmentService;

        public DoctorsController(IDoctorServices doctorServices,IAppointmentService appointmentService)
        {
            _doctorServices = doctorServices;
            _appointmentService = appointmentService;
        }

        [HttpGet("")]

        public async Task<IActionResult> GetWorkingDay()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var result = await _doctorServices.GetDoctorWorkingDays(userId);
            return Ok(result);
        }

        [HttpPost("")]

        public async Task<IActionResult> AddWorkingDay([FromBody] AddDayRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var result = await _doctorServices.AddWorkingDay(userId, request);
            return Ok(result);
        }

        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateWorkingDay([FromRoute] int id,[FromBody] UpdateDayOfWorkRequest request)
        {
           
            var result = await _doctorServices.UpdateworkingDay(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteWorkingDay([FromRoute] int id)
        {

            var result = await _doctorServices.DeleteWorkingDayAsync(id);
            return Ok(result);
        }

        [HttpGet("Appointments")]

        public async Task<IActionResult> GetDoctorAppointments([FromQuery] Status? Status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var result = await _appointmentService.GetDoctorAppointments(userId, Status);
            return Ok(result);
        }

        [HttpPatch("Appointments")]

        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] UpdateAppointmentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.UpdateAppointmentStatus(request,userId);
            return Ok(result);
        }

        [HttpPatch("PatientData/{id}")]

        public async Task<IActionResult> UpdatePatientData([FromBody] UpdatePatientDataRequest request, [FromRoute] int id)
        {
            var result = await _doctorServices.UpdatePatientData(request, id);
            return Ok(result);
        }

        [HttpGet("Patient")]

        public async Task<IActionResult> GetPatientByIdOrName([FromQuery] int? Id, [FromQuery] string? Name)
        {

            var result = await _doctorServices.GetPatientByIdOrName(Id, Name);
            return Ok(result);
        }

    }
}
