namespace MeetingsApplication.Models
{
    public class MeetingModel
    {
        public int         Id { get; set; }
        public List<int>   Participants { get; set; } = new List<int>();
        public DateTime    StartTime { get; set; }
        public DateTime    EndTime { get; set; }
    }
}
