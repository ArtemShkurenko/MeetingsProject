using MeetingsApplication.DAL;
using MeetingsApplication.Models;

namespace MeetingsApplication.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public void Create(User user)
        {
            _userRepository.Create(user);
        }

        public User GetById(int userId)
        {
            return _userRepository.GetRecordById(userId);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }
    }
}
