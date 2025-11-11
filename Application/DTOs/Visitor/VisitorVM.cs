using Application.DTOs.Vehicle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Visitor
{
    public class VisitorVM
    {
        public int Id{ get; set; }
        public string FullName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public DateTime RegisterDateTime { get; set; } = DateTime.Now;
        public string GuidCode { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsInside { get; set; } = true;
        public DateTime? ExitDateTime { get; set; }

        public VehicleDto Vehicles { get; set; }
    }
}
