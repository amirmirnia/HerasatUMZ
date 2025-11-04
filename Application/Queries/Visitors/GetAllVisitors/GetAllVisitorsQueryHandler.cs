using Application.DTOs.Visitor;
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
    }
}
