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
            var user = await manager.CheckCredentials(viewModel,
                viewModel.Password, cancellationToken);

            // give JWT token
            manager.CreateAuthenticationTicket(user, HttpContext.Session);

            return Ok();
        }

        /// <summary>
        /// Only authorized user can create other user.
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