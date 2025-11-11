using Domain.Common;
using Domain.Entities.Visitors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities.Vehicles
{
    public class Vehicle : BaseEntity
    {
        [Required]
        public string PlatePart1 { get; set; } = string.Empty;

        [Required]
        public string PlatePart2 { get; set; } = string.Empty;

        [Required]
        public string PlatePart3 { get; set; } = string.Empty; 

        [Required]
        public string PlatePart4 { get; set; } = string.Empty; 

        public VehicleType? VehicleType { get; set; } 
        public string? Color { get; set; }
        public string? Brand { get; set; }

        public DateTime EntryDateTime { get; set; } = DateTime.Now;
        public DateTime? ExitDateTime { get; set; }
        public bool IsInside { get; set; } = true;

        public string? PlatePhotoPath { get; set; }

        [ForeignKey(nameof(Visitor))]
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;

    }
}
