using Application.DTOs;
using Application.DTOs.Log;
using Client.Services.Interface;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;


namespace Client.Services.Repository
{
    public class VisitLogger : IVisitLogger
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient http { get; }
        private readonly NavigationManager _nav;

        public VisitLogger(IHttpClientFactory httpClientFactory, NavigationManager nav)
        {
            _httpClientFactory = httpClientFactory;
            http = _httpClientFactory.CreateClient("HerasatUmz.ServerAPI");
            _nav = nav;
        }

        public async Task LogAsync(VisitLogDto log)
        {

            log.Page = _nav.Uri.Replace(_nav.BaseUri, "").Split('?')[0];
            if (log.Page!=null)
            {
                try
                {
                    await http.PostAsJsonAsync("api/Visit", log);
                }
                catch { }
            }

           
        }

        public async Task<PagedResult<VisitLogResponseDto>> GetAllLogAsync(int? pageNumber, int? pageSize, string? CodeId)
        {
            var visitLogs=new PagedResult<VisitLogResponseDto>();
            try
            {
                visitLogs = await http.GetFromJsonAsync<PagedResult<VisitLogResponseDto>>($"api/visit?pageNumber={pageNumber} & pageSize={pageSize}");
            }
            catch
            {
                
            }
            return visitLogs;
        }

        public async Task<PagedResult<VisitLogResponseDto>> GetAllLogUserAsync(int? pageNumber, int? pageSize)
        {
            var visitLogs = new PagedResult<VisitLogResponseDto>();
            try
            {
                visitLogs = await http.GetFromJsonAsync<PagedResult<VisitLogResponseDto>>($"api/visit/GetAllLogUser?pageNumber={pageNumber} & pageSize={pageSize}");
            }
            catch
            {

            }
            return visitLogs;
        }
    }
}
