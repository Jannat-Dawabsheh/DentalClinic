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
        private readonly IManageService _manageService;

        public ManagesController(IManageService manageService)
        {
            _manageService = manageService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var response = await _manageService.GetAllDoctorsAsync();
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult>CreateDoctor(CreateDoctorRequest request)
        {
            var response=await _manageService.CreateAsync(request);
            return  Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
        {
            var result = await _manageService.DeleteDoctorAsync(id);
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
            var result = await _manageService.UpdateDoctorAsync(id, request);
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


        [HttpGet("DashboardSummary")]
        public async Task<IActionResult> GetDashboardSummaryAsync([FromQuery] int? month)
        {
            var result=await _manageService.GetDashboardSummaryAsync(month);
            return Ok(result);
        }

        [HttpGet("visitsDetails")]
        public async Task<IActionResult> GetVisitsDetails([FromQuery] int? month)
        {
            var result = await _manageService.GetDashboardResponseAsync(month);
            return Ok(result);
        }
    }
}
