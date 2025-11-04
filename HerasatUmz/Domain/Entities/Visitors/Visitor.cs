using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Visitors
{
    public class Visitor : BaseEntity
    {

        public string FullName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public DateTime RegisterDateTime { get; set; } = DateTime.Now;
        public string GuidCode { get; set; } = Guid.NewGuid().ToString("N")[..10]; // 10 رقمی تصادفی
        public string? PhoneNumber { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsInside { get; set; } = true;
        public DateTime? ExitDateTime { get; set; } 
    }
}
