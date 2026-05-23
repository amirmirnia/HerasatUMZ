using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Visitor.ExitVisitor
{
    public class ExitVisitorCommandHandler : IRequestHandler<ExitVisitorCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ExitVisitorCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(ExitVisitorCommand request, CancellationToken cancellationToken)
        {
            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(v => v.Id == request.Id && v.IsInside, cancellationToken);

            if (visitor == null)
                throw new Exception("ملاقات‌شونده یافت نشد.");

            // Only the registrar can mark exit, unless the caller is Admin/Manager.
            var currentId = _currentUser.UserId;
            if (!_currentUser.IsPrivileged &&
                !string.Equals(visitor.CreatedBy, currentId, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException(
                    "فقط کاربری که این ملاقات‌کننده را ثبت کرده می‌تواند خروج او را ثبت کند.");
            }

            visitor.IsInside = false;
            visitor.ExitDateTime = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
