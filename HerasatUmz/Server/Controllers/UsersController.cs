using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Commands.Users.RegisterUser;
using Application.Commands.Users.UpdateUser;
using Application.Commands.Users.DeleteUser;
using Application.Queries.Users.GetUserById;
using Application.Queries.Users.GetAllUsers;
using Application.DTOs.User;
using Domain.Enum;
using Application.Commands.Users.ActiveUser;
using Application.Queries.Users.GetUserByIdcode;
using Application.DTOs;
using Application.Queries.Users.UserStats;

namespace Server.Controllers;

/// <summary>
/// Controller for managing users and authentication
/// </summary>
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}")]
public class UsersController : BaseApiController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<UserDto>> Register(RegisterUserCommand command)
    {
        var user = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPost("registerUser")]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(400)]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> registerUser(RegisterUserCommand command)
    {
        var user = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserListDto>), 200)]
    public async Task<ActionResult<List<UserListDto>>> GetAllUsers(
        [FromQuery] string? searchQuery = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? status = null,
        [FromQuery] string? selectedCompany = null,
        [FromQuery] UserRole? selectedRole = null)
    {
        var query = new GetAllUsersQuery
        {
            pageNumber = pageNumber,
            pageSize = pageSize,
            searchQuery = searchQuery,
            selectedCompany = selectedCompany,
            selectedRole = selectedRole,
            status = status
        };

        var users = await Mediator.Send(query);
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await Mediator.Send(new GetUserByIdQuery(id));
        return HandleResult(user);
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(401)]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var currentUserId = GetCurrentUserId();
        var user = await Mediator.Send(new GetUserByIdcodeQuery(currentUserId));
        return HandleResult(user);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    [ProducesResponseType(400)]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserCommand command)
    {
        if (id != command.Id)
            throw new ArgumentException("شناسه مسیر با شناسه بدنه درخواست یکسان نیست.");

        var currentUserId = GetCurrentUserId();
        var currentUserRole = GetCurrentUserRole();

        if (currentUserRole != UserRole.Admin.ToString() && currentUserRole != UserRole.Manager.ToString())
        {
            if (currentUserId != command.CodeId)
                throw new UnauthorizedAccessException("شما اجازه ویرایش این کاربر را ندارید.");
        }

        var user = await Mediator.Send(command);
        return HandleResult(user);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await Mediator.Send(new DeleteUserCommand { Id = id });
        return HandleBooleanResult(result);
    }

    [HttpGet("stats")]
    [ProducesResponseType(typeof(UserStatsDto), 200)]
    public async Task<ActionResult<UserStatsDto>> GetUserStats()
    {
        var stats = await Mediator.Send(new GetUserStatsQuery());
        return Ok(stats);
    }

    [HttpPost("{id}/change-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<ActionResult> ChangePassword(int id, [FromBody] ResetpasswordUserCommand request)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRole = GetCurrentUserRole();

        if (currentUserRole != UserRole.Admin.ToString() && currentUserRole != UserRole.Manager.ToString())
        {
            if (currentUserId != request.IdCode.ToString())
                throw new UnauthorizedAccessException("شما اجازه تغییر رمز عبور این کاربر را ندارید.");
        }

        await Mediator.Send(request);
        return Ok(new { Message = "رمز عبور با موفقیت تغییر کرد." });
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult ForgotPassword([FromBody] ResetPasswordRequest request)
    {
        // TODO: Implement ForgotPasswordCommand when created
        return Ok(new { Message = "این قابلیت هنوز پیاده‌سازی نشده است." });
    }

    [HttpPost("ActiveUser")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> ActiveUser(ActiveUserCommand command)
    {
        await Mediator.Send(command);
        return Ok(new { Message = "کاربر با موفقیت فعال شد." });
    }
}

public class ChangePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordConfirmRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class VerifyEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
