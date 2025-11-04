using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.Visitor;
using AutoMapper;
using Domain.Entities.Visitors;

namespace Application.Commands.Visitor.RegisterVisitor;

public class RegisterVisitorCommandHandler : IRequestHandler<RegisterVisitorCommand, VisitorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    public RegisterVisitorCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<VisitorDto> Handle(RegisterVisitorCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Visitors
            .FirstOrDefaultAsync(v => v.NationalCode == request.NationalCode && v.IsInside, cancellationToken);

        if (exists != null)
            throw new InvalidOperationException("بازدیدکننده‌ای با این کد ملی درون سازمان هست.");

        var visitor = new Domain.Entities.Visitors.Visitor
        {
            FullName = request.FullName,
            NationalCode = request.NationalCode,
            PhoneNumber = request.PhoneNumber,
            HostName= request.HostName,
            GuidCode = Guid.NewGuid().ToString("N")[..10],
            RegisterDateTime=DateTime.UtcNow,
            IsActive=true,
            IsInside=true,
            PhotoPath="",
            
        };

        _context.Visitors.Add(visitor);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VisitorDto>(visitor);
    }
}
