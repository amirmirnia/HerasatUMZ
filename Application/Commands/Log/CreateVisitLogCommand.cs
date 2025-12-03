using MediatR;


namespace Application.Commands.Log
{
    public class CreateVisitLogCommand : IRequest<bool>
    {
        public string? UserName { get; set; }
        public string? Codeid { get; set; }
        public string? Ip { get; set; }
        public string? Page { get; set; }
        public string? EventType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
