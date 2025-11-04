using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.Visitor;
using Application.DTOs;
using Domain.Enum;
using Application.Queries.Visitors.GetAllVisitors;
using Application.Commands.Visitor.RegisterVisitor;
using Application.Commands.Visitor.ExitVisitor;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Controllers;


[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}")]
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
        try
        {
            var visitor = await Mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("VisitorRegistered", visitor);


            return Ok(visitor);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    [HttpPost("GetAllVisitors")]
    public async Task<IActionResult> GetAllVisitors(SearchVisitorsDro model)
    {
        try
        {
            var query = new GetAllVisitorsQueryHandler()
            {
                IsInside = model.IsInside,
                searchQuery= model.searchQuery,
                EnterTime= model.EnterTime,
                ExitTime=model.ExitTime

            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {

            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("ExitVisitor")]
    public async Task<IActionResult> ExitVisitor([FromBody] ExitVisitorCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            if (result)
            {
                await _hubContext.Clients.All.SendAsync("VisitorExited", command.Id);

                return Ok(new { Message = "ملاقات‌شونده با موفقیت خارج شد." });
            }
            else
            {

                return BadRequest(new { Message = "خطا در ثبت خروج." });
            }
        }
        catch (Exception ex)
        {

            return BadRequest(new { Message = ex.Message });
        }
    }
    ///// <summary>
    ///// دریافت تمام بازدیدکنندگان
    ///// </summary>
    //[HttpGet]
    //[ProducesResponseType(typeof(PagedResult<VisitorListDto>), 200)]
    //public async Task<ActionResult> GetAllVisitors(
    //    [FromQuery] string? search = null,
    //    [FromQuery] int pageNumber = 1,
    //    [FromQuery] int pageSize = 10)
    //{
    //    var query = new GetAllVisitorsQuery
    //    {
    //        Search = search,
    //        PageNumber = pageNumber,
    //        PageSize = pageSize
    //    };

    //    var visitors = await Mediator.Send(query);
    //    return Ok(visitors);
    //}

    ///// <summary>
    ///// دریافت جزئیات بازدیدکننده
    ///// </summary>
    //[HttpGet("{id}")]
    //[ProducesResponseType(typeof(VisitorDto), 200)]
    //[ProducesResponseType(404)]
    //public async Task<ActionResult> GetVisitor(int id)
    //{
    //    var visitor = await Mediator.Send(new GetVisitorByIdQuery(id));
    //    return HandleResult(visitor);
    //}
}
