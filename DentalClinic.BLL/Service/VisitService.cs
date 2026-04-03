using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly IPatientRepository _patientRepository;

        public VisitService(IFileService fileServices,IVisitRepository visitRepository,
            UserManager<ApplicationUser> userManager, IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _fileServices = fileServices;
            _visitRepository = visitRepository;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
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

            var cratedVisit=await _visitRepository.Createvisit(visit);
            //if (cratedVisit is not null) { 
            //    var request=new BookAppointmentRequest
            //    {
            //        StartDateTime= (DateTime)cratedVisit.NextAppointmentDate,
            //        EndDateTime= (DateTime)cratedVisit.NextAppointmentDate + cratedVisit.,

            //    }
            //}
            var user = await _userManager.FindByIdAsync(visit.CreatedBy);
            if (user is not null)
            {
                visit.CreatedBy = user.FullName;
            }
      
            return visit.Adapt<VisitResponse>();
        }

        public async Task<List<VisitResponseForPatient>?>GetAllVisitsForPatient(string userId)
        {
            var patient = await _patientRepository.FindByIdAsync(userId);
            if (patient == null)
                return null;
            var visits= await _visitRepository.GetAllVisitsForPatient(patient.Id);
            if (visits == null || !visits.Any())
                return null;



            var doctorIds = visits.Select(v => v.CreatedBy).Distinct().ToList();


            var doctors = _userManager.Users .Where(u => doctorIds.Contains(u.Id)).ToDictionary(u => u.Id, u => u.FullName);

            foreach (var visit in visits)
            {
                if (doctors.TryGetValue(visit.CreatedBy, out var doctorName))
                {
                    visit.CreatedBy = doctorName;
                }
            }
            return visits.Adapt<List<VisitResponseForPatient>>();
            
     
        }

        public async Task<VisitDetailsForPatient?>GetVisitDetailsForPatient(string userId,int visitId)
        {
            var patient = await _patientRepository.FindByIdAsync(userId);
            if (patient == null)
                return null;
            var visit = await _visitRepository.GetVisitDetailsForPatient(patient.Id,visitId);
            if (visit is null )
                return null;

            var user = await _userManager.FindByIdAsync(visit.CreatedBy);

            visit.CreatedBy = user.FullName;
            

            return visit.Adapt<VisitDetailsForPatient>();

        }

        public async Task<List<VisitResponseForDoctor>?> GetAllVisitsForDoctor(string userId,int? patientId)
        {
            var doctor = await _doctorRepository.FindByIdAsync(userId);
            if (doctor == null)
                return null;
            var visits = await _visitRepository.GetAllVisitsForDoctor(doctor.Id, patientId);
            if (visits == null || !visits.Any())
                return null;



           return visits.Adapt<List<VisitResponseForDoctor>>();


        }

        public async Task<VisitDetailsForDoctor?> GetVisitDetailsForDoctor( int visitId)
        {

            var visit = await _visitRepository.GetVisitDetailsForDoctr(visitId);
            if (visit is null)
                return null;

            var user = await _userManager.FindByIdAsync(visit.CreatedBy);

            visit.CreatedBy = user.FullName;


            return visit.Adapt<VisitDetailsForDoctor>();

        }

        public async Task<List<VisitResponseForPatient>?> GetPatientVisitsForDoctor(int patientId)
        {
            var patient = await _patientRepository.FindByPatientIdAsync(patientId);
            if (patient == null)
                return null;
            var visits = await _visitRepository.GetAllVisitsForPatient(patient.Id);
            if (visits == null || !visits.Any())
                return null;


            var doctorIds = visits.Select(v => v.CreatedBy).Distinct().ToList();


            var doctors = _userManager.Users.Where(u => doctorIds.Contains(u.Id)).ToDictionary(u => u.Id, u => u.FullName);

            foreach (var visit in visits)
            {
                if (doctors.TryGetValue(visit.CreatedBy, out var doctorName))
                {
                    visit.CreatedBy = doctorName;
                }
            }

            return visits.Adapt<List<VisitResponseForPatient>>();


        }

    }
}
