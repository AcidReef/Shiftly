namespace Shiftly.DTOs
{
    public class LeaveRequestCreateDto
    {
        public string UserId { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Reason { get; set; }
    }
}