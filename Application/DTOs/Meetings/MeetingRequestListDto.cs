namespace Application.DTOs.Meetings;

public class MeetingRequestListDto
{
    public int Id { get; set; }
    public string HostFullName { get; set; } = "";
    public string HostNationalCode { get; set; } = "";
    public string VisitorFullName { get; set; } = "";
    public DateOnly RequestedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = "";
}

public class MeetingRequestPageDto
{
    public List<MeetingRequestListDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = 20;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
