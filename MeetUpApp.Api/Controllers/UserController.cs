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

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] UserViewModel viewModel,
            CancellationToken cancellationToken)
        {
            var user = await manager.CheckCredentialsAsync(viewModel,
                viewModel.Password, cancellationToken);

            manager.CreateAuthenticationTicket(user, HttpContext.Session);

            return Ok();
        }

        /// <summary>
        /// Only authorized user can create other user.
        /// </summary>
        [Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] UserViewModel viewModel,
            CancellationToken cancellationToken)
        {
            await manager.AddUserAsync(
                viewModel.Username,
                viewModel.Password,
                cancellationToken);

            return Ok();
        }

        [HttpGet(nameof(Logout))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Ok();
        }
    }
}