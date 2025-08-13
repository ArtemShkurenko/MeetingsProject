namespace MeetingsApplication.Models
{
    public class ScheduleResponse
    {
        public DateTime ProposedStart { get; set; }
        public DateTime ProposedEnd { get; set; }
        public List<string> EdgeCases { get; set; } = new List<string>();
    }
}
