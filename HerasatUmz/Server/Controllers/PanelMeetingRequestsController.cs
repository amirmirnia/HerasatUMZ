using Application.Queries.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Authorize]
[Route("api/panel/meeting-requests")]
public class PanelMeetingRequestsController : BaseApiController
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await Mediator.Send(new GetMeetingRequestByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search = null,
        [FromQuery] int pageNumber = 1,
        CancellationToken cancellationToken = default)
        => Ok(await Mediator.Send(
            new GetMeetingRequestsQuery(search, pageNumber), cancellationToken));
}
