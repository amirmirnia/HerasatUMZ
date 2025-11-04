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
using System.Security.Claims;
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
    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<UserDto>> Register(RegisterUserCommand command)
    {
        try
        {
            var user = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("registerUser")]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(400)]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> registerUser(RegisterUserCommand command)
    {
        try
        {
            var user = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }



    /// <summary>
    /// Get all users with optional filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserListDto>), 200)]
    public async Task<ActionResult<List<UserListDto>>> GetAllUsers(
        [FromQuery] string searchQuery = null,
         [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? status = null,
        [FromQuery] string? selectedCompany = null,
        [FromQuery] UserRole? selectedRole = null)
    {
        var isAuth = User.Identity?.IsAuthenticated;  // آیا True است؟
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var roled = User.FindFirstValue(ClaimTypes.Role);
        try
        {
            var query = new GetAllUsersQuery
            {
                pageNumber= pageNumber,
                pageSize= pageSize,
                searchQuery= searchQuery,
                selectedCompany = selectedCompany,
                selectedRole = selectedRole,
                status = status
            };

            var users = await Mediator.Send(query);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            // Check if user can access this profile (self or admin/manager)
            //var currentUserRole = GetCurrentUserRole();
            //var currentUserId = GetCurrentUserId();
            
            //if (currentUserRole != "Admin" && currentUserRole != "Manager" && currentUserId != id.ToString())
            //{
            //    return Forbid();
            //}

            var user = await Mediator.Send(new GetUserByIdQuery(id));
            return HandleResult(user);
        }
        catch (Application.Common.Exceptions.NotFoundException)
        {
            return NotFound(new { Message = $"User with ID {id} not found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(401)]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        try
        {
           var currentUserId = GetCurrentUserId();
            var user = await Mediator.Send(new GetUserByIdcodeQuery(currentUserId));
            return HandleResult(user);
        }
        catch (Application.Common.Exceptions.NotFoundException)
        {
            return NotFound(new { Message = "Current user not found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Update user information
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    [ProducesResponseType(400)]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { Message = "ID mismatch between route and body." });

        var currentUserId = GetCurrentUserId();
        var currentUserRole = GetCurrentUserRole();
        // اگر کاربر عادی است فقط خودش را می‌تواند ویرایش کند
        if (currentUserRole != UserRole.Admin.ToString() && currentUserRole != UserRole.Manager.ToString())
        {
            if (currentUserId != command.CodeId)
                return Forbid();
        }
        

        var user = await Mediator.Send(command);
        return HandleResult(user);
    }
    /// <summary>
    /// Delete user (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteUserCommand { Id = id });
            return HandleBooleanResult(result);
        }
        catch (Application.Common.Exceptions.NotFoundException)
        {
            return NotFound(new { Message = $"User with ID {id} not found." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    /// <summary>
    /// دریافت آمار کلی کاربران (تعداد کل، فعال و مدیران)
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(UserStatsDto), 200)]
    public async Task<ActionResult<UserStatsDto>> GetUserStats()
    {
        try
        {
            var stats = await Mediator.Send(new GetUserStatsQuery());
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [HttpPost("{id}/change-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<ActionResult> ChangePassword(int id,[FromBody] ResetpasswordUserCommand request)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != UserRole.Admin.ToString() && currentUserRole != UserRole.Manager.ToString())
            {
                if (currentUserId != request.IdCode.ToString())
                {
                    return Forbid();
                }
            }
             


            await Mediator.Send(request);
            return Ok(new { Message = "Password changed successfully." });

        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Request password reset
    /// </summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> ForgotPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            // TODO: Implement ForgotPasswordCommand when created
            // await Mediator.Send(new ForgotPasswordCommand { Email = request.Email });
            // return Ok(new { Message = "If the email exists, a reset link has been sent." });

            return Ok(new { Message = "Password reset functionality not implemented yet." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }



    [HttpPost("ActiveUser")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> ActiveUser(ActiveUserCommand command)
    {
        try
        {

            await Mediator.Send(command);
            return Ok(new { Message = "user verified successfully." });

        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}



/// <summary>
/// Request models for user operations
/// </summary>
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