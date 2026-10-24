using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Meetings;

public class CreateMeetingRequestDto
{
    [Required, StringLength(200)]
    public string HostFullName { get; set; } = "";

    public string HostNationalCode { get; set; } = "";

    [Required, StringLength(200)]
    public string VisitorFullName { get; set; } = "";

    [Required]
    public DateOnly? RequestedDate { get; set; }
}
