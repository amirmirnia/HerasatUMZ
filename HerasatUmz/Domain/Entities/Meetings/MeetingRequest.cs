using Domain.Common;

namespace Domain.Entities.Meetings;

public class MeetingRequest : BaseEntity
{
    public string HostFullName { get; set; } = "";
    public string HostNationalCode { get; set; } = "";
    public string VisitorFullName { get; set; } = "";
    public DateOnly RequestedDate { get; set; }
    public string Status { get; set; } = "Pending";
}
