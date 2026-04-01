using DentalClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DentalClinic.DAL.DTO.Response.Patient
{
    public class VisitResponseForPatient
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
    }
}
