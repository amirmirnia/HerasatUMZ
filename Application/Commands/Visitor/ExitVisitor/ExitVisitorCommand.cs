using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Visitor.ExitVisitor
{
    public class ExitVisitorCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
