using Application.DTOs.Vehicle;
using Domain.Enums;
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

        /// <summary>نوع ارباب رجوع (فرد عادی، دانشجو و ...)</summary>
        public VisitorType Type { get; set; } = VisitorType.Normal;

        /// <summary>توضیحات تکمیلی</summary>
        public string? Description { get; set; }

        public DateTime RegisterDateTime { get; set; } = DateTime.Now;
        public string GuidCode { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsInside { get; set; } = true;
        public DateTime? ExitDateTime { get; set; }

        /// <summary>IdCode of the user who registered this visitor (from CreatedBy audit field).</summary>
        public string? RegisteredByIdCode { get; set; }

        /// <summary>Display name of the registering user.</summary>
        public string? RegisteredByName { get; set; }

        public VehicleDto Vehicles { get; set; }
    }
}
