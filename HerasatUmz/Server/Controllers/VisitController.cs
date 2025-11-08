using Application.Commands.Log;
using Application.DTOs.Log;
using Application.Queries.Log.GetAllLog;
using Domain.Entities.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Server.Controllers
{
    [ApiController]
    [EnableCors("AllowBlazor")]
    [Route("api/[controller]")]
    [Authorize]

    public class VisitController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VisitLogDto dto)
        {
            var ip = GetClientIp(HttpContext);

            var command = new CreateVisitLogCommand
            {
                Codeid = GetCurrentUserId(),
                UserName = GetCurrentFullName(),
                Ip = ip ?? "unknown",
                Page = dto.Page,
                EventType = dto.EventType,
                Timestamp = dto.Timestamp ?? DateTime.Now
            };

            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);

            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Error logging visit");
                return StatusCode(500, new { success = false, message = "Error logging visit" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(
             [FromQuery] string searchQuery = null,
         [FromQuery] int? pageNumber = 1,
        [FromQuery] int? pageSize = 10)
        {
            var command = new GetVisitLogsQuery()
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                searchQuery = searchQuery,
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        private static string? GetClientIp(HttpContext ctx)
        {
            // اگر پشت پروکسی هستید، بررسی X-Forwarded-For لازم است
            if (ctx.Request.Headers.TryGetValue("X-Forwarded-For", out var vals))
            {
                var first = vals.FirstOrDefault();
                if (!string.IsNullOrEmpty(first)) return first.Split(',')[0].Trim();
            }
            return ctx.Connection.RemoteIpAddress?.ToString();
        }
    }
}
