using DentalClinic.BLL.Service;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response.Doctor;
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

        public DoctorsController(IDoctorServices doctorServices)
        {
            _doctorServices = doctorServices;
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
    }
}
