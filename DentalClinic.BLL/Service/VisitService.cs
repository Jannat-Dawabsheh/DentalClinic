using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class VisitService: IVisitService
    {
        private readonly IFileService _fileServices;
        private readonly IVisitRepository _visitRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDoctorRepository _doctorRepository;

        public VisitService(IFileService fileServices,IVisitRepository visitRepository,UserManager<ApplicationUser> userManager, IDoctorRepository doctorRepository)
        {
            _fileServices = fileServices;
            _visitRepository = visitRepository;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
        }

        public async Task<BaseResponse>CheckVisitData(int id,List<int>? medicineIds,DateTime? NextAppointmentDate)
        {
            return await _visitRepository.CheckVisitData(id, medicineIds, NextAppointmentDate);
        }
        public async Task<VisitResponse> CreateVisit(AddVisitRequest request)

        {


            var visit = request.Adapt<Visit>();

            if (request.XRayImages != null)
            {
                visit.XRayImages = new List<XRayImage>();
                foreach (var file in request.XRayImages)
                {
                    var imagePath = await _fileServices.UploadAsync(file);
                    visit.XRayImages.Add(new XRayImage { ImageName = imagePath });
                }
            }

            await _visitRepository.Createvisit(visit);
            var user = await _userManager.FindByIdAsync(visit.CreatedBy);
            if (user is not null)
            {
                visit.CreatedBy = user.FullName;
            }
      
            return visit.Adapt<VisitResponse>();
        }
    }
}
