using Domain.Entities.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Log
{
    public class VisitLogFilterDto
    {
        public string? UserName { get; set; }
        public string? Codeid { get; set; }
        public string? Ip { get; set; }
        public string? Page { get; set; }
        public string? EventType { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
