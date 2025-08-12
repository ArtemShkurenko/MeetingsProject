namespace MeetingsApplication.Models
{
    public class User : IRecord
    {
        public int           Id { get; set; }
        public string        Name { get; set; }
       // public List<Meeting> meetings { get; set; } = new List<Meeting>();
    }
}
