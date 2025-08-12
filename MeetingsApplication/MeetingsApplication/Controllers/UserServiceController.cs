using MeetingsApplication.DAL;
using MeetingsApplication.Models;
using MeetingsApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserServiceController : ControllerBase
    {
        public  readonly UserService     _userService;
        private readonly MeetingsService _meetingsService;
        public UserServiceController (UserService userService, MeetingsService meetingsService)
        {
            _userService     = userService;
            _meetingsService = meetingsService;
        }

        [HttpGet("GetAllUser")]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

        [HttpPost("Create")]
        public IActionResult Create(UserModel userModel)
        {
            var user = new User
            {
                Name = userModel.Name
            };
            _userService.Create(user);
            return Accepted();
        }
        [HttpGet("{userId}/meetings")]
        public IActionResult GetMeetingsForUser(int userId)
        {
            var user = _userService.GetById(userId);
            if (user == null)
                return NotFound();

            var meetings = _meetingsService.GetMeetingsForUser(userId);
            return Ok(meetings);
        }
    }
}
