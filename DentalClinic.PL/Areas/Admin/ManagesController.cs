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
    public class ManagesController : ControllerBase
    {
        private readonly IManageService _doctorService;

        public ManagesController(IManageService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var response = await _doctorService.GetAllDoctorsAsync();
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult>CreateDoctor(CreateDoctorRequest request)
        {
            var response=await _doctorService.CreateAsync(request);
            return  Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] int id, [FromBody] UpdateDoctorRequest request)
        {
            var result = await _doctorService.UpdateDoctorAsync(id, request);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
