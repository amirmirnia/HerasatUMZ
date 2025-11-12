using Application.Common.Interfaces;
using Application.DTOs;
using Application.DTOs.Visitor;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Visitors.GetAllVisitors
{
    public class GetAllVisitorsHandler : IRequestHandler<GetAllVisitorsQueryHandler, PagedResult<VisitorVM>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllVisitorsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<VisitorVM>> Handle(GetAllVisitorsQueryHandler request, CancellationToken cancellationToken)
        {
            var query = _context.Visitors.Include(x=>x.Vehicles)
                .AsNoTracking()
                .Where(v => v.IsInside == request.IsInside).AsQueryable();

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


            var pageNumber = request.pageNumber ?? 1;
            var pageSize = request.pageSize ?? 10;

            var totalCount = await query.CountAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (pageNumber > totalPages && totalPages > 0)
                pageNumber = totalPages;
            else if (totalPages == 0)
                pageNumber = 1;





            var visitors = await query
                .OrderByDescending(v => v.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);


            return new PagedResult<VisitorVM>
            {
                Items = _mapper.Map<List<VisitorVM>>(visitors),
                TotalCount = totalCount
            };
        }
    }
}
