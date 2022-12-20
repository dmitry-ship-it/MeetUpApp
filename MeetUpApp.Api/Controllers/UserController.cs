using MeetUpApp.Managers;
using MeetUpApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetUpApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager manager;

        public UserController(UserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(
            [FromBody] UserViewModel viewModel,
            CancellationToken cancellationToken)
        {
            // TODO: Move to manager
            var user = await manager.GetAsync(
                u => u.Name == viewModel.Username, cancellationToken);

            manager.CheckCredentials(user, viewModel.Password);

            // give JWT token
            manager.CreateAuthenticationTicket(user, HttpContext.Session);

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
            await manager.AddUser(
                viewModel.Username,
                viewModel.Password,
                cancellationToken);

            // TODO: Move messgate to Constants
            return Ok("You have successfully registered.");
        }

        [HttpGet(nameof(Logout))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logged out.");
        }
    }
}
