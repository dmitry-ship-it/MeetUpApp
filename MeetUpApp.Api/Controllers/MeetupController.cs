using AutoMapper;
using MeetUpApp.Api.Data.DAL;
using MeetUpApp.Api.Data.Models;
using MeetUpApp.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace MeetUpApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetupController : Controller
    {
        private readonly IRepository<Meetup> meetupRepository;
        private readonly IMapper mapper;
        private readonly ILogger<MeetupController> logger;

        public MeetupController(
            IRepository<Meetup> meetupRepository,
            IMapper mapper,
            ILogger<MeetupController> logger)
        {
            this.meetupRepository = meetupRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet(nameof(All))]
        public async Task<IActionResult> All(
            CancellationToken cancellationToken)
        {
            return Ok(await meetupRepository.GetAllAsync(cancellationToken));
        }

        [Authorize]
        [HttpPut(nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] MeetupViewModel meetup,
            CancellationToken cancellationToken)
        {
            var dbModel = mapper.Map<MeetupViewModel, Meetup>(meetup);
            mapper.Map(meetup.Address, dbModel);

            try
            {
                await meetupRepository.InsertAsync(dbModel, cancellationToken);
            }
            catch (DbException)
            {
                logger.LogInformation(
                    "User '{Name}' tried to create ne meetup but failed.",
                    HttpContext.User.Identity!.Name);
                return BadRequest("Invalid meetup data.");
            }

            logger.LogInformation("User '{Name}' created a new meetup.",
                HttpContext.User.Identity!.Name);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Select(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            var meetup = await meetupRepository.GetByIdAsync(id, cancellationToken);

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
            var dbModel = mapper.Map<MeetupViewModel, Meetup>(meetup);
            mapper.Map(meetup.Address, dbModel);
            dbModel.Id = id;

            try
            {
                await meetupRepository.UpdateAsync(dbModel, cancellationToken);
            }
            catch (Exception ex) when (ex is DbException or DbUpdateConcurrencyException)
            {
                logger.LogInformation(
                    "User '{Name}' tried to update meetup but this meetup was not found.",
                    HttpContext.User.Identity!.Name);
                return BadRequest("This meetup was not found.");
            }

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
            var dbModel = await meetupRepository.GetByIdAsync(id, cancellationToken);

            if (dbModel is null)
            {
                logger.LogInformation(
                    "User '{Name}' tried to delete meetup with id={Id} but this meetup doesn't exists.",
                    HttpContext.User.Identity!.Name, id);

                return BadRequest("This meetup was not found.");
            }

            await meetupRepository.RemoveAsync(dbModel, cancellationToken);

            logger.LogInformation("User '{Name}' has deleted meetup with id={Id}.",
                HttpContext.User.Identity!.Name, id);

            return Accepted();
        }
    }
}
