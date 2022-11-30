using AutoMapper;
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
    public class MeetupController : Controller
    {
        private readonly IRepository<Meetup> _meetupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MeetupController> _logger;

        public MeetupController(
            IRepository<Meetup> meetupRepository,
            IMapper mapper,
            ILogger<MeetupController> logger)
        {
            _meetupRepository = meetupRepository;
            _mapper = mapper;
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
            var dbModel = _mapper.Map<MeetupViewModel, Meetup>(meetup);
            _mapper.Map(meetup.Address, dbModel);

            try
            {
                await _meetupRepository.InsertAsync(dbModel, cancellationToken);
            }
            catch (DbException)
            {
                _logger.LogInformation("User '{Name}' tried to create ne meetup but failed.",
                    HttpContext.User.Identity!.Name);
                return BadRequest("Invalid meetup data.");
            }

            _logger.LogInformation("User '{Name}' created a new meetup.",
                HttpContext.User.Identity!.Name);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Select(
            [FromQuery] int id,
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
            [FromQuery] int id,
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            var dbModel = _mapper.Map<MeetupViewModel, Meetup>(meetup);
            _mapper.Map(meetup.Address, dbModel);
            dbModel.Id = id;

            try
            {
                await _meetupRepository.UpdateAsync(dbModel, cancellationToken);
            }
            catch (DbException)
            {
                _logger.LogInformation("User '{Name}' tried to update meetup but this meetup was not found.",
                    HttpContext.User.Identity!.Name);
                return BadRequest("This meetup was not found.");
            }

            _logger.LogInformation("User '{Name}' updated meetup with id={Id}.",
                HttpContext.User.Identity!.Name, id);

            return Accepted();
        }

        [Authorize]
        [HttpDelete(nameof(Delete))]
        public async Task<IActionResult> Delete(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            var dbModel = await _meetupRepository.GetByIdAsync(id, cancellationToken);

            if (dbModel is null)
            {
                _logger.LogInformation(
                    "User '{Name}' tried to delete meetup with id={Id} but this meetup doesn't exists.",
                    HttpContext.User.Identity!.Name, id);

                return BadRequest("This meetup was not found.");
            }

            await _meetupRepository.RemoveAsync(dbModel, cancellationToken);

            _logger.LogInformation("User '{Name}' has deleted meetup with id={Id}.",
                HttpContext.User.Identity!.Name, id);

            return Accepted();
        }
    }
}
