using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.Visitor;
using Application.Queries.Visitors.GetAllVisitors;
using Application.Commands.Visitor.RegisterVisitor;
using Application.Commands.Visitor.ExitVisitor;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class VisitorsController : BaseApiController
{
    private readonly IHubContext<VisitorHub> _hubContext;

    public VisitorsController(IHubContext<VisitorHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(VisitorDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<VisitorDto>> RegisterVisitor(RegisterVisitorCommand command)
    {
        var visitor = await Mediator.Send(command);
        await _hubContext.Clients.All.SendAsync("VisitorRegistered", visitor);
        return Ok(visitor);
    }

    [HttpPost("GetAllVisitors")]
    public async Task<IActionResult> GetAllVisitors(SearchVisitorsDro model)
    {
        var query = new GetAllVisitorsQueryHandler
        {
            IsInside = model.IsInside,
            searchQuery = model.searchQuery,
            EnterTime = model.EnterTime,
            ExitTime = model.ExitTime,
            pageNumber = model.pageNumber,
            pageSize = model.pageSize
        };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("ExitVisitor")]
    public async Task<IActionResult> ExitVisitor([FromBody] ExitVisitorCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result)
            return BadRequest(new Application.Common.Errors.ApiError
            {
                Code = "exit_failed",
                Message = "ثبت خروج با مشکل مواجه شد."
            });

        await _hubContext.Clients.All.SendAsync("VisitorExited", command.Id);
        return Ok(new { Message = "ملاقات‌شونده با موفقیت خارج شد." });
    }
}
