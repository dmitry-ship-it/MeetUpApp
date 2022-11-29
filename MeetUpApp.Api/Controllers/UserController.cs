using MeetUpApp.Api.Authentication;
using MeetUpApp.Api.Data.DAL;
using MeetUpApp.Api.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeetUpApp.Api.Controllers
{
    public class UserController
    {
        private readonly IRepository<User> _userRepository;
        private readonly UserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IRepository<User> userRepository,
            UserManager userManager,
            ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Login()
        {

        }
    }
}
