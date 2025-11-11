using Application.DTOs.Visitor;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Visitors.GetAllVisitors
{
    public class GetAllVisitorsQueryHandler : IRequest<List<VisitorVM>>
    {
        public bool IsInside { get; set; }
        public string searchQuery { get; set; }
        public DateTime? EnterTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string? PhotoPath { get; set; }

        public string? PlatePart1 { get; set; }
        public PlateLetter? PlateLetter { get; set; }
        public string? PlatePart3 { get; set; }
        public string? PlatePart4 { get; set; }
        public string? PhotovehiclePath { get; set; }

    }
}
