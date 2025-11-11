using AutoMapper;

using Application.DTOs.User;
using Domain.Entities.Users;
using Domain.Entities.Visitors;
using Application.DTOs.Visitor;
using Application.DTOs.Log;
using Domain.Entities.Log;
using Domain.Entities.Vehicles;
using Application.DTOs.Vehicle;


namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserListDto>();
        CreateMap<Visitor, VisitorDto>();
        CreateMap<Visitor, VisitorVM>();
        CreateMap<VisitLog, VisitLogResponseDto>();
        CreateMap<Vehicle, VehicleDto>();

    }
}