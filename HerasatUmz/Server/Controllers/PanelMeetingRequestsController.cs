using Application.Queries.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Authorize]
[Route("api/panel/meeting-requests")]
public class PanelMeetingRequestsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search = null,
        [FromQuery] int pageNumber = 1,
        CancellationToken cancellationToken = default)
        => Ok(await Mediator.Send(
            new GetMeetingRequestsQuery(search, pageNumber), cancellationToken));
}
