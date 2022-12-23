using MeetUpApp.Managers;
using MeetUpApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetUpApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetupController : Controller
    {
        private readonly MeetupManager manager;

        public MeetupController(MeetupManager manager)
        {
            this.manager = manager;
        }

        [HttpGet("All")]
        public async Task<IActionResult> AllAsync(
            CancellationToken cancellationToken)
        {
            return Ok(await manager.GetAllAsync(cancellationToken));
        }

        [Authorize]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync(
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            await manager.AddAsync(meetup, cancellationToken);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> SelectAsync(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            var meetup = await manager.GetAsync(id, cancellationToken);

            return Ok(meetup);
        }

        [Authorize]
        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateAsync(
            [FromQuery] int id,
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            await manager.UpdateAsync(id, meetup, cancellationToken);

            return Accepted();
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            await manager.RemoveAsync(id, cancellationToken);

            return Accepted();
        }
    }
}