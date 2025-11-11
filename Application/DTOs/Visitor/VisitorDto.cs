using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Visitor
{
    public class VisitorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PhotoBase64 { get; set; }


        public bool HasVehicle { get; set; } = false;
        public string? PlatePart1 { get; set; }
        public PlateLetter? PlateLetter { get; set; }
        public string? PlatePart3 { get; set; }
        public string? PlatePart4 { get; set; }
        public VehicleType? VehicleType { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public string? VehiclePhotoBase64 { get; set; }
    }
}
