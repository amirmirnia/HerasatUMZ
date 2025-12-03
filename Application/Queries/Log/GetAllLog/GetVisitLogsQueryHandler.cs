using Application.Common.Interfaces;
using Application.DTOs;
using Application.DTOs.Log;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Log.GetAllLog
{
    public class GetVisitLogsQueryHandler : IRequestHandler<GetVisitLogsQuery, PagedResult<VisitLogResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetVisitLogsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<VisitLogResponseDto>> Handle(GetVisitLogsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.visitLogs.AsQueryable();

            if (!string.IsNullOrEmpty(request.CodeId))
                query = query.Where(x => x.Codeid.Contains(request.CodeId));

            if (!string.IsNullOrEmpty(request.searchQuery))
                query = query.Where(x => x.UserName == request.searchQuery);

            if (!string.IsNullOrEmpty(request.searchQuery))
                query = query.Where(x => x.Codeid == request.searchQuery);

            if (!string.IsNullOrEmpty(request.searchQuery))
                query = query.Where(x => x.Ip == request.searchQuery);

            if (!string.IsNullOrEmpty(request.searchQuery))
                query = query.Where(x => x.Page == request.searchQuery);

            if (!string.IsNullOrEmpty(request.searchQuery))
                query = query.Where(x => x.EventType == request.searchQuery);



            // ------------------ صفحه‌بندی ------------------
            var pageNumber = request.pageNumber ?? 1;
            var pageSize = request.pageSize ?? 10;

            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0)
                pageNumber = totalPages;
            else if (totalPages == 0)
                pageNumber = 1;

            var logs = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // ------------------ مپ و بازگشت ------------------
            var items = _mapper.Map<List<VisitLogResponseDto>>(logs);

            return new PagedResult<VisitLogResponseDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
