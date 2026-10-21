using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.Visitor;
using AutoMapper;
using Domain.Entities.Visitors;
using Domain.Entities.Vehicles;
using Application.Services.Image;

namespace Application.Commands.Visitor.RegisterVisitor;

public class RegisterVisitorCommandHandler : IRequestHandler<RegisterVisitorCommand, VisitorVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IMeetingRequestRegistrationGuard _guard;
    public RegisterVisitorCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser,
        IMeetingRequestRegistrationGuard guard)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
        _guard = guard;
    }

    public async Task<VisitorVM> Handle(RegisterVisitorCommand request, CancellationToken cancellationToken)
    {
        if (request.MeetingRequestId is int id)
        {
            return await _guard.ExecuteAsync($"entry:{id}",
                () => RegisterAsync(request, cancellationToken), cancellationToken);
        }

        return await RegisterAsync(request, cancellationToken);
    }

    private async Task<VisitorVM> RegisterAsync(RegisterVisitorCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Meetings.MeetingRequest? meeting = null;
        if (request.MeetingRequestId is int meetingId)
        {
            meeting = await _context.MeetingRequests.FirstOrDefaultAsync(
                x => x.Id == meetingId && x.IsActive, cancellationToken)
                ?? throw new NotFoundException("درخواست ملاقات یافت نشد.");

            if (meeting.Status == "Entered")
                throw new InvalidOperationException("ورود این درخواست قبلاً ثبت شده است.");
        }

        // بررسی وجود بازدیدکننده داخل سازمان
        var exists = await _context.Visitors.Include(x=>x.Vehicles)
            .FirstOrDefaultAsync(v => ((v.NationalCode == request.NationalCode)
            || (v.Vehicles.PlatePart1 == request.PlatePart1
            && v.Vehicles.PlatePart3 == request.PlatePart3
            && v.Vehicles.PlatePart4 == request.PlatePart4
            && v.Vehicles.PlateLetter == request.PlateLetter)) && v.IsInside, cancellationToken);

        if (exists != null)
            throw new InvalidOperationException("بازدیدکننده‌ای با این کد ملی یا پلاک درون سازمان هست.");

        var visitor = new Domain.Entities.Visitors.Visitor
        {
            FullName = request.FullName,
            NationalCode = request.NationalCode,
            PhoneNumber = request.PhoneNumber,
            HostName = request.HostName,
            Type = request.Type,
            Description = request.Description,
            GuidCode = Guid.NewGuid().ToString("N")[..10],
            RegisterDateTime = DateTime.Now,
            IsActive = true,
            IsInside = true,
            PhotoPath = null,
            RegisteredByName = _currentUser.UserName
        };

        // ذخیره عکس اگر وجود داشته باشه
        if (!string.IsNullOrWhiteSpace(request.PhotoBase64))
        {
            visitor.PhotoPath=await SaveImageAsync.SaveAsync(request.PhotoBase64, "wwwroot/uploads/visitors");

        }

        _context.Visitors.Add(visitor);
        if (meeting != null)
            meeting.Status = "Entered";
        await _context.SaveChangesAsync(cancellationToken);


        // --- ثبت ماشین اگر داشته باشه ---
        if (request.HasVehicle)
        {
            var vehicle = new Vehicle
            {
                VisitorId = visitor.Id,
                PlatePart1 = request.PlatePart1!,
                PlateLetter = request.PlateLetter,
                PlatePart3 = request.PlatePart3!,
                PlatePart4 = request.PlatePart4!,
                VehicleType = request.VehicleType,
                Color = request.Color,
                Brand = request.Brand,
                EntryDateTime = DateTime.UtcNow,
            };

            // ذخیره عکس پلاک
            if (!string.IsNullOrWhiteSpace(request.VehiclePhotoBase64))
            {
                vehicle.VehiclePhotoPath = await SaveImageAsync.SaveAsync(request.VehiclePhotoBase64, "wwwroot/uploads/Vehiclevisitors");
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return _mapper.Map<VisitorVM>(visitor);
    }
}
