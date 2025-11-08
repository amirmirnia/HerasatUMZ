using Application.DTOs.Visitor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Visitor.RegisterVisitor
{
    public class RegisterVisitorCommand : IRequest<VisitorDto>
    {
        public string FullName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PhotoBase64 { get; set; }
    }
}
