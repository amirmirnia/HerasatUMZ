using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Enum;
using Application.DTOs.Dashboard;
using Application.Queries.Rooms.GetAllOverviewDashboard;

namespace Server.Controllers;


[Route("api/[controller]")]
[Authorize]
public class DashboardController : BaseApiController
{

    [HttpGet("overview")]
    [ProducesResponseType(typeof(DashboardOverview), 200)]
    public async Task<ActionResult<DashboardOverview>> GetDashboardOverview()
    {
        try
        {
            var overview = await Mediator.Send(new GetAllOverviewDashboardQuery());
            return Ok(overview);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}


