using Application.Common.Interfaces;
using Application.DTOs.Visitor;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Visitors.GetAllVisitors
{
    public class GetAllVisitorsHandler : IRequestHandler<GetAllVisitorsQueryHandler, List<VisitorVM>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllVisitorsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VisitorVM>> Handle(GetAllVisitorsQueryHandler request, CancellationToken cancellationToken)
        {
            var query = _context.Visitors
                .AsNoTracking()
                .Where(v => v.IsInside == request.IsInside);

            // 🔍 جستجو بر اساس نام، کد ملی یا میزبان
            if (!string.IsNullOrWhiteSpace(request.searchQuery))
            {
                var keyword = request.searchQuery.Trim();
                query = query.Where(v =>
                    v.FullName.Contains(keyword) ||
                    v.NationalCode.Contains(keyword) ||
                    v.HostName.Contains(keyword));
            }

            // ⏰ فیلتر بازه زمانی
            if (request.EnterTime.HasValue && request.ExitTime.HasValue)
            {
                var start = request.EnterTime.Value.Date;
                var end = request.ExitTime.Value.Date.AddDays(1); // تا انتهای روز دوم

                query = query.Where(v =>
                    v.RegisterDateTime >= start &&
                    v.RegisterDateTime < end);
            }
            else if (request.EnterTime.HasValue)
            {
                query = query.Where(v => v.RegisterDateTime >= request.EnterTime.Value);
            }
            else if (request.ExitTime.HasValue)
            {
                query = query.Where(v => v.RegisterDateTime <= request.ExitTime.Value);
            }

            var visitors = await query
                .OrderByDescending(v => v.RegisterDateTime)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<VisitorVM>>(visitors);
        }
    }
}
