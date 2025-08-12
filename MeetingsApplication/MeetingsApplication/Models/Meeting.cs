namespace MeetingsApplication.Models
{
    public class Meeting : IRecord
    {
        public int         Id { get; set; }
        public List<int>   Participants { get; set; } = new List<int>();
        public DateTime    StartTime { get; set; }
        public DateTime    EndTime { get; set; }
    }
}
