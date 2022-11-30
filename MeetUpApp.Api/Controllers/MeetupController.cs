using MeetUpApp.Api.Data.DAL;
using MeetUpApp.Api.Data.Models;
using MeetUpApp.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Threading;

namespace MeetUpApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetupController : Controller
    {
        private readonly IRepository<Meetup> _meetupRepository;
        private readonly ILogger<MeetupController> _logger;

        public MeetupController(
            IRepository<Meetup> meetupRepository,
            ILogger<MeetupController> logger)
        {
            _meetupRepository = meetupRepository;
            _logger = logger;
        }

        [HttpGet(nameof(All))]
        public async Task<IActionResult> All(
            CancellationToken cancellationToken)
        {
            return Ok(await _meetupRepository.GetAllAsync(cancellationToken));
        }

        [Authorize]
        [HttpPut(nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            // TODO: replace with AutoMapper call
            var dbModel = new Meetup();

            try
            {
                await _meetupRepository.InsertAsync(dbModel, cancellationToken);
            }
            catch (DbException)
            {
                return BadRequest("Invalid meetup data.");
            }

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Select(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var meetup = await _meetupRepository.GetByIdAsync(id, cancellationToken);

            if (meetup is null)
            {
                return BadRequest($"Meetup with id={id} was not found");
            }

            return Ok(meetup);
        }

        [Authorize]
        [HttpPatch(nameof(Update))]
        public async Task<IActionResult> Update(
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            // TODO: replace with AutoMapper call
            var dbModel = new Meetup();

            try
            {
                await _meetupRepository.UpdateAsync(dbModel, cancellationToken);
            }
            catch (DbException)
            {
                return BadRequest("This meetup was not found.");
            }

            return Accepted();
        }

        [Authorize]
        [HttpDelete(nameof(Delete))]
        public async Task<IActionResult> Delete(
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            // TODO: replace with AutoMapper call
            var dbModel = new Meetup();

            try
            {
                await _meetupRepository.RemoveAsync(dbModel, cancellationToken);
            }
            catch (DbException)
            {
                return BadRequest("This meetup was not found.");
            }

            return Accepted();
        }
    }
}
