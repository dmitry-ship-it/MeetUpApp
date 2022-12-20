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

        [HttpGet(nameof(All))]
        public async Task<IActionResult> All(
            CancellationToken cancellationToken)
        {
            return Ok(await manager.GetAllAsync(cancellationToken));
        }

        [Authorize]
        [HttpPost(nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            await manager.AddAsync(meetup, cancellationToken);
            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Select(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            var meetup = await manager.GetAsync(id, cancellationToken);
            return Ok(meetup);
        }

        [Authorize]
        [HttpPatch(nameof(Update))]
        public async Task<IActionResult> Update(
            [FromQuery] int id,
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            await manager.UpdateAsync(id, meetup, cancellationToken);
            return Accepted();
        }

        [Authorize]
        [HttpDelete(nameof(Delete))]
        public async Task<IActionResult> Delete(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            await manager.RemoveAsync(id, cancellationToken);
            return Accepted();
        }
    }
}
