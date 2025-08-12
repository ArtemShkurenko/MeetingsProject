using MeetingsApplication.DAL;
using MeetingsApplication.Models;

namespace MeetingsApplication.Services
{
    public class MeetingsService
    {
        private readonly IRepository<Meeting> _meetingRepository;
        private readonly TimeSpan             BusinessStart = TimeSpan.FromHours(9);
        private readonly TimeSpan             BusinessEnd = TimeSpan.FromHours(17);

        public MeetingsService(IRepository<Meeting> meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }
       
        public void Create(Meeting meeting)
        {
            _meetingRepository.Create(meeting);
        }

        public Meeting GetById(int meetingId)
        {
            return _meetingRepository.GetRecordById(meetingId);
        }
        public IEnumerable<Meeting> GetMeetingsForUser(int userId) 
        {
            return _meetingRepository.GetAll()
                .Where(m => m.Participants.Contains(userId));
        }   
        public IEnumerable<Meeting> GetAll()
        {
            return _meetingRepository.GetAll();
        }
        public ScheduleResponse? FindEarliestSlot(List<int> participantIds, int durationMinutes, DateTime earliestStartUtc, DateTime latestEndUtc)
        {
            if (!participantIds.Any()) return null;
            var startBound    = earliestStartUtc;
            var endBound      = latestEndUtc;
            var busyIntervals = _meetingRepository.GetAll()
                .Where(m => m.Participants.Any(p => participantIds.Contains(p)))
                .Select(m => Tuple.Create(m.StartTime.ToUniversalTime(), m.EndTime.ToUniversalTime()))
                .Where(t => t.Item2 > earliestStartUtc && t.Item1 < latestEndUtc)
                .ToList();

            var required = TimeSpan.FromMinutes(durationMinutes);

            for (var day = earliestStartUtc.Date; day <= latestEndUtc.Date; day = day.AddDays(1))
            {
                var dayStart = day.Add(BusinessStart);
                var dayEnd   = day.Add(BusinessEnd);

                if (dayStart < earliestStartUtc) dayStart = earliestStartUtc;
                if (dayEnd > latestEndUtc) dayEnd = latestEndUtc;
                if (dayStart >= dayEnd) continue;

                var merged = MergeIntervals(
                    busyIntervals
                        .Where(t => t.Item1 < dayEnd && t.Item2 > dayStart)
                        .Select(t => Tuple.Create(
                            t.Item1 < dayStart ? dayStart : t.Item1,
                            t.Item2 > dayEnd ? dayEnd : t.Item2
                        ))
                        .ToList()
                );

                var pointer = dayStart;

                foreach (var interval in merged)
                {
                    if (pointer + required <= interval.Item1)
                        return new ScheduleResponse 
                        { 
                            ProposedStart = pointer, 
                            ProposedEnd = pointer + required 
                        };

                    if (pointer < interval.Item2)
                        pointer = interval.Item2;
                }

                if (pointer + required <= dayEnd)
                    return new ScheduleResponse 
                    { 
                        ProposedStart = pointer, 
                        ProposedEnd = pointer + required
                    };
            }

            return null;
        }
        private List<Tuple<DateTime, DateTime>> MergeIntervals(List<Tuple<DateTime, DateTime>> intervals)
        {
            var result = new List<Tuple<DateTime, DateTime>>();
            if (!intervals.Any()) return result;

            var sorted = intervals.OrderBy(i => i.Item1).ToList();
            var curStart = sorted[0].Item1;
            var curEnd = sorted[0].Item2;

            for (int i = 1; i < sorted.Count; i++)
            {
                if (sorted[i].Item1 <= curEnd)
                    curEnd = sorted[i].Item2 > curEnd ? sorted[i].Item2 : curEnd;
                else
                {
                    result.Add(Tuple.Create(curStart, curEnd));
                    curStart = sorted[i].Item1;
                    curEnd = sorted[i].Item2;
                }
            }
            result.Add(Tuple.Create(curStart, curEnd));
            return result;
        }
    }
}
