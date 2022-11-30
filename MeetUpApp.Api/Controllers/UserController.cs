using MeetUpApp.Api.Authentication;
using MeetUpApp.Api.Data.DAL;
using MeetUpApp.Api.Data.Models;
using MeetUpApp.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace MeetUpApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
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

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(
            [FromBody] UserViewModel viewModel,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByExpressionAsync(
                u => u.Name == viewModel.Username, cancellationToken);

            if (user is null)
            {
                _logger.LogInformation(
                    "Attempted to log in as '{Name}' but this user was not found.",
                    viewModel.Username);

                return NotFound($"User '{viewModel.Username}' not found.");
            }

            if (!_userManager.CheckCredentials(user, viewModel.Password))
            {
                _logger.LogInformation(
                    "Attempted to log in as '{Name}' but password is invalid.",
                    viewModel.Username);

                return BadRequest("Username or password is invalid.");
            }

            // give JWT token
            _userManager.CreateAuthenticationTicket(user, HttpContext.Session);

            _logger.LogInformation(
                "User '{Name}' successfully logged in.",
                    viewModel.Username);

            return Ok("You have successfully logged in.");
        }

        /// <summary>
        /// Only authorized use can create other user.
        /// </summary>
        /// <param name="viewModel">View model with username and password</param>
        /// <param name="cancellationToken"></param>
        [Authorize]
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(
            [FromBody] UserViewModel viewModel,
            CancellationToken cancellationToken)
        {
            var user = _userManager.CreateUser(
                viewModel.Username, viewModel.Password);

            try
            {
                await _userRepository.InsertAsync(user, cancellationToken);
            }
            catch (DbException)
            {
                _logger.LogWarning(
                    "Can't create new user '{Name}' (DB error).",
                    viewModel.Username);

                return BadRequest("Can't create new user with this username.");
            }

            _logger.LogInformation(
                "User '{Name}' successfully created.",
                    viewModel.Username);

            return Ok("You have successfully registered.");
        }

        [HttpGet(nameof(Logout))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            if (HttpContext.User?.Identity is not null)
            {
                _logger.LogInformation("User '{Name}' logged out",
                    HttpContext.User!.Identity.Name);
            }

            HttpContext.Session.Clear();
            return Ok("Logged out.");
        }
    }
}
