using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Vehicle
{
    public class VehicleDto
    {
        public string PlatePart1 { get; set; } = string.Empty;
        public PlateLetter? PlateLetter { get; set; }
        public string PlatePart3 { get; set; } = string.Empty;
        public string PlatePart4 { get; set; } = string.Empty;
        public VehicleType? VehicleType { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public string? VehiclePhotoPath { get; set; }

    }
}
