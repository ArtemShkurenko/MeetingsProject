using MeetingsApplication.DAL;
using MeetingsApplication.Models;
using MeetingsApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace MeetingsApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingServiceController : ControllerBase
    {
        public readonly MeetingsService _meetingService;
        public MeetingServiceController(MeetingsService meetingService)
        {
            _meetingService = meetingService;
        }
        [HttpGet("GetAllMeetings")]
        public IEnumerable<Meeting> Get()
        {
            return _meetingService.GetAll();
        }

        [HttpPost("Create")]
        public IActionResult Create(MeetingModel meetingModel)
        {
            var meeting = new Meeting
            {
                Participants = meetingModel.Participants,
                StartTime = meetingModel.StartTime.ToUniversalTime(),
                EndTime = meetingModel.EndTime.ToUniversalTime()
            };
            _meetingService.Create(meeting);
            return Accepted();
        }
        [HttpPost("find-slot")]
        public IActionResult FindSlot(ScheduleRequest request)
        {
            var slot = _meetingService.FindEarliestSlot(
                request.ParticipantIds,
                request.DurationMinutes,
                request.EarliestStart.ToUniversalTime(),
                request.LatestEnd.ToUniversalTime()
            );

            if (slot == null)
                return NotFound(new { message = "No available slot found." });

            return Ok(slot);
        }
    }
}
