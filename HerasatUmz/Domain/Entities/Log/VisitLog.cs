using Domain.Common;
using Domain.Enum;

namespace Domain.Entities.Log
{
    public class VisitLog:BaseEntity
    {
        public string? UserName { get; set; }
        public string? Codeid { get; set; }
        public string Ip { get; set; } = default!;
        public string Page { get; set; } = default!; 
        public string EventType { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
   
}
