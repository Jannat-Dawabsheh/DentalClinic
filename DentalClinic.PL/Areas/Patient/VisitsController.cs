using DentalClinic.BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DentalClinic.PL.Areas.Patient
{
    [Route("api/Patient/[controller]")]
    [ApiController]
    [Authorize]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;


        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;

        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllVisitForPatient()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _visitService.GetAllVisitsForPatient(userId);
            return Ok(response);
        }

        [HttpGet("{visitId}")]
        public async Task<IActionResult> GetVisitDetailsForPatient([FromRoute] int visitId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _visitService.GetVisitDetailsForPatient(userId,visitId);
            return Ok(response);
        }
    }
}
