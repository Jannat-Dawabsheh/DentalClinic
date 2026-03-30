using DentalClinic.BLL.Service;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace DentalClinic.PL.Areas.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IDoctorServices _doctorServices;

        public VisitsController(IVisitService visitService,IDoctorServices doctorServices)
        {
            _visitService = visitService;
            _doctorServices = doctorServices;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] AddVisitRequest request)
        {
            var medicineIds = request.Medicines?.Select(m => m.MedicineId).ToList();
            var result = await _visitService.CheckVisitData(request.AppointmentId, medicineIds,request.NextAppointmentDate);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }



            var response = await _visitService.CreateVisit(request);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updateRequest = new UpdateAppointmentRequest
            {
                id = request.AppointmentId,
                status = Status.Completed
            };
            await _doctorServices.UpdateAppointmentStatus(updateRequest, userId);
            return Ok(response);
        }
    }
}
