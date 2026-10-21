using Application.Commands.Meetings;
using Application.DTOs.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication;

namespace Server.Controllers;

[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
[Route("api/meeting-requests")]
public class MeetingRequestsController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMeetingRequestDto dto,
        CancellationToken cancellationToken)
    {
        var id = await Mediator.Send(new CreateMeetingRequestCommand(
            dto.HostFullName, dto.HostNationalCode,
            dto.VisitorFullName, dto.RequestedDate!.Value), cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new
        {
            success = true,
            message = "درخواست ملاقات با موفقیت ثبت شد.",
            id,
            status = "Pending"
        });
    }
}
