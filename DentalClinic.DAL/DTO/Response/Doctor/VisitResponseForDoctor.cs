using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Doctor
{
    public class VisitResponseForDoctor
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
    }
}
