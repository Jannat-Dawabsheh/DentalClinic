using DentalClinic.DAL.Data;
using DentalClinic.DAL.DTO.Request.Doctor;
using DentalClinic.DAL.DTO.Request.Patient;
using DentalClinic.DAL.DTO.Response;
using DentalClinic.DAL.DTO.Response.Doctor;
using DentalClinic.DAL.DTO.Response.Patient;
using DentalClinic.DAL.Migrations;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
        private readonly IEmailSender _emailSender;
        private readonly IPatientService _patientService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly ApplicationDbContext _context;

        public VisitService(IFileService fileServices,IVisitRepository visitRepository,
            UserManager<ApplicationUser> userManager, IDoctorRepository doctorRepository,
            IPatientRepository patientRepository, IEmailSender emailSender,IPatientService patientService,
            IAppointmentRepository appointmentRepository,IAppointmentService appointmentService,
            ApplicationDbContext context)
        {
            _fileServices = fileServices;
            _visitRepository = visitRepository;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _emailSender = emailSender;
            _patientService = patientService;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _context = context;
        }

        public async Task<BaseResponse>CheckVisitData(int id,List<int>? medicineIds,DateTime? NextAppointmentDate)
        {
            return await _visitRepository.CheckVisitData(id, medicineIds, NextAppointmentDate);
        }
        public async Task<VisitResponse> CreateVisit(AddVisitRequest request)

        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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



                Appointment appointment = null;
                Doctor doctor = null;
                Patient patient = null;




                if (request.NextAppointmentDate.HasValue)
                {
                    var existingAppointment = await _visitRepository.GetAppointmentById(request.AppointmentId);

                    if (existingAppointment == null)
                        throw new Exception("Appointment not found");

                    var doctorId = existingAppointment.DoctorId;
                    var patientId = existingAppointment.PatientId;

                    var appointmentDate = request.NextAppointmentDate.Value;
                    var dayOfWeek = appointmentDate.DayOfWeek;




                    var schedule = await _doctorRepository.GetWorkingDayForDoctorByDay(doctorId, appointmentDate.DayOfWeek);

                    if (schedule == null)
                        throw new Exception("Doctor not working on this day");



                    var end = appointmentDate.AddMinutes(schedule.AppointmentDuration);

                    var doctorSchedual = await _appointmentRepository.isAvailable(doctorId, appointmentDate);

                    if (doctorSchedual == null)
                        throw new Exception("Doctor schedule not found");

                    var slots = await _appointmentService.GetAvilableSlots(doctorSchedual.Id);

                    if (slots?.Slots == null || !(slots.Slots.Any(s => s.StartDateTime <= appointmentDate && s.EndDateTime > appointmentDate)))
                    {
                        throw new Exception("Selected time slot is not available");

                    }



                    var appointmentRequest = new BookAppointmentRequest
                    {
                        StartDateTime = appointmentDate,
                        EndDateTime = end
                    };

                    if (appointmentRequest.IsValid(out var error))
                    {

                        appointment = appointmentRequest.Adapt<Appointment>();
                        appointment.DoctorId = doctorId;
                        appointment.PatientId = patientId;
                        appointment.Status = Status.Confirmed;

                        doctor = await _doctorRepository.FindByDoctorIdAsync(doctorId);
                        patient = await _patientRepository.FindByPatientIdAsync(patientId);




                    }

                }

                    var createdVisit = await _visitRepository.Createvisit(visit);

                    if (appointment != null)
                    {
                        await _appointmentRepository.BookAppointment(appointment.DoctorId, appointment);
                    }

                    await transaction.CommitAsync();

                    if (appointment != null)
                    {
                        await _emailSender.SendEmailAsync(patient.User.Email, "New appointment",
                            $"<p>Hello {patient.User.UserName}, there is a new appointment booked by doctor: {doctor.User.FullName}</p>"
                        );
                    }

                    var user = await _userManager.FindByIdAsync(visit.CreatedBy);
                    if (user is not null)
                    {
                        visit.CreatedBy = user.FullName;
                    }


                    return visit.Adapt<VisitResponse>();
                
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

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
