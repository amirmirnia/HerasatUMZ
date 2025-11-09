using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult HandleResult<T>(T result)
    {
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    protected ActionResult HandleListResult<T>(IEnumerable<T> result)
    {
        var list = result?.ToList();
        if (list == null || !list.Any())
            return Ok(new List<T>());

        return Ok(list);
    }

    protected ActionResult HandleBooleanResult(bool result)
    {
        return result ? Ok(new { Success = true }) : BadRequest(new { Success = false });
    }

    protected string GetCurrentUserId()
    {
        return User.FindFirst("sub")?.Value ??
               User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ??
               "Anonymous";
    }

    protected string GetCurrentFullName()
    {
        return User.FindFirst("Name")?.Value ??
               User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value ??
               string.Empty;
    }

    protected string GetCurrentUserRole()
    {
        return User.FindFirst("role")?.Value ??
               User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value ??
               "User";
    }
}