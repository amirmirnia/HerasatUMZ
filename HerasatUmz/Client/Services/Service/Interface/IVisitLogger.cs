using Application.DTOs;
using Application.DTOs.Log;

namespace Client.Services.Interface
{
    public interface IVisitLogger
    {
        Task LogAsync(VisitLogDto log);
        Task<PagedResult<VisitLogResponseDto>> GetAllLogAsync(int? pageNumber, int? pageSize,string? CodeId);
        Task<PagedResult<VisitLogResponseDto>> GetAllLogUserAsync(int? pageNumber, int? pageSize);
    }
}
