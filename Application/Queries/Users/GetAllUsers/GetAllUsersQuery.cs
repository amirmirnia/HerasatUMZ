using MediatR;
using Application.DTOs.User;
using Domain.Enum;
using Application.DTOs;

namespace Application.Queries.Users.GetAllUsers;

public class GetAllUsersQuery : IRequest<PagedResult<UserListDto>>
{
    public bool? status { get; set; }
    public string? selectedCompany { get; set; }
    public UserRole? selectedRole { get; set; }
    public int? pageSize { get; set; }
    public int? pageNumber { get; set; }
    public string searchQuery { get; set; }
}
