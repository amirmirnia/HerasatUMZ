using Application.DTOs;
using Application.DTOs.Log;
using MediatR;


namespace Application.Queries.Log.GetAllLog
{

    public class GetVisitLogsQuery : IRequest<PagedResult<VisitLogResponseDto>>
    {
        public string? CodeId { get; set; }
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string searchQuery { get; set; }
    }
}
