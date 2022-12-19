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
        private readonly ILogger<MeetupController> logger;

        public MeetupController(
            MeetupManager manager,
            ILogger<MeetupController> logger)
        {
            this.manager = manager;
            this.logger = logger;
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
            //logger.LogInformation(
            //    "User '{Name}' tried to create the meetup but failed.",
            //    HttpContext.User.Identity!.Name);

            logger.LogInformation("User '{Name}' created a new meetup.",
                HttpContext.User.Identity!.Name);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Select(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            var meetup = await manager.GetAsync(id, cancellationToken);

            // return BadRequest($"Meetup with id={id} was not found");

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

            //logger.LogInformation(
            //    "User '{Name}' tried to update meetup but this meetup was not found.",
            //    HttpContext.User.Identity!.Name);
            //return BadRequest("This meetup was not found.");

            logger.LogInformation("User '{Name}' updated meetup with id={Id}.",
                HttpContext.User.Identity!.Name, id);

            return Accepted();
        }

        [Authorize]
        [HttpDelete(nameof(Delete))]
        public async Task<IActionResult> Delete(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            await manager.RemoveAsync(id, cancellationToken);

            logger.LogInformation("User '{Name}' has deleted meetup with id={Id}.",
                HttpContext.User.Identity!.Name, id);

            return Accepted();
        }
    }
}
