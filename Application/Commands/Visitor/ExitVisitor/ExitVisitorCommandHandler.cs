using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Visitor.ExitVisitor
{
    public class ExitVisitorCommandHandler : IRequestHandler<ExitVisitorCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ExitVisitorCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ExitVisitorCommand request, CancellationToken cancellationToken)
        {
            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(v => v.Id == request.Id  && v.IsInside, cancellationToken);

            if (visitor == null)
                throw new Exception("ملاقات‌شونده یافت نشد.");

            visitor.IsInside = false;
            visitor.ExitDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
