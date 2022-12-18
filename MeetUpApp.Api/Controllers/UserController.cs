﻿using MeetUpApp.Api.Managers;
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
        private readonly UserManager manager;
        private readonly ILogger<UserController> logger;

        public UserController(
            UserManager manager,
            ILogger<UserController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(
            [FromBody] UserViewModel viewModel,
            CancellationToken cancellationToken)
        {
            var user = await manager.GetAsync(
                u => u.Name == viewModel.Username, cancellationToken);

            if (user is null)
            {
                logger.LogInformation(
                    "Attempted to log in as '{Name}' but this user was not found.",
                    viewModel.Username);

                return NotFound($"User '{viewModel.Username}' not found.");
            }

            if (!manager.CheckCredentials(user, viewModel.Password))
            {
                logger.LogInformation(
                    "Attempted to log in as '{Name}' but password is invalid.",
                    viewModel.Username);

                return BadRequest("Username or password is invalid.");
            }

            // give JWT token
            manager.CreateAuthenticationTicket(user, HttpContext.Session);

            logger.LogInformation(
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
            try
            {
                await manager.AddUser(
                    viewModel.Username,
                    viewModel.Password,
                    cancellationToken);
            }
            catch (DbException)
            {
                logger.LogWarning(
                    "Can't create new user '{Name}' (DB error).",
                    viewModel.Username);

                return BadRequest("Can't create new user with this username.");
            }

            logger.LogInformation(
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
                logger.LogInformation("User '{Name}' logged out",
                    HttpContext.User!.Identity.Name);
            }

            HttpContext.Session.Clear();
            return Ok("Logged out.");
        }
    }
}
