using MediatR;
using AutoMapper;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

using Application.DTOs.Dashboard;
using Domain.Enum;

namespace Application.Queries.Rooms.GetAllOverviewDashboard;

public class GetAllOverviewDashboardQueryHandler : IRequestHandler<GetAllOverviewDashboardQuery, DashboardOverview>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllOverviewDashboardQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DashboardOverview> Handle(GetAllOverviewDashboardQuery request, CancellationToken cancellationToken)
    {
        return new DashboardOverview()
        {
            ActiveUsers =await _context.Users.Where(p => p.IsActive).CountAsync(),
            TotalUsers= await _context.Users.CountAsync(),
            AllVisitors = await _context.Visitors.CountAsync(),
            AllVisitorsInArea = await _context.Visitors.Where(p=>p.IsInside).CountAsync(),



        };
    }
}