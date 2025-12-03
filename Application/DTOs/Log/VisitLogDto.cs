using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Log
{
    public class VisitLogDto
    {
        public string? Page { get; set; }
        public string? EventType { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
