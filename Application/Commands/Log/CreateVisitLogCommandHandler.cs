using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities.Log;
using MediatR;

namespace Application.Commands.Log
{
    public class CreateVisitLogCommandHandler : IRequestHandler<CreateVisitLogCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateVisitLogCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CreateVisitLogCommand request, CancellationToken cancellationToken)
        {
            var log = new VisitLog
            {
                Codeid = request.Codeid ?? null,
                UserName = request.UserName,
                Ip = request.Ip,
                Page = request.Page,
                EventType = request.EventType,
                Timestamp = request.Timestamp,
            };

            _context.visitLogs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
