using AutoMapper;
using MeetUpApp.Api.Controllers;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.Managers;
using MeetUpApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MeetUpApp.Api.Tests
{
    public class MeetupControllerTests
    {
        private readonly Mock<MeetupManager> managerMock;
        private readonly MeetupController controller;

        public MeetupControllerTests()
        {
            managerMock = new Mock<MeetupManager>(
                Mock.Of<IRepository<Meetup>>(), Mock.Of<IMapper>());

            controller = new MeetupController(managerMock.Object);
        }

        [Fact]
        public async Task MeetupControllerAllAsyncCallsManager()
        {
            //Arrange
            managerMock.Setup(m => m.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(GetTestMeetups);

            //Act
            var result = await controller.AllAsync(CancellationToken.None);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should()
                .BeEquivalentTo(GetTestMeetups());
        }

        [Fact]
        public async Task MeetupControllerSelectAsyncCallsManager()
        {
            //Arrange
            const int id = 1;
            var returns = GetTestMeetups().Single(m => m.Id == id);
            managerMock.Setup(m => m.GetAsync(1, CancellationToken.None))
                .ReturnsAsync(returns);

            //Act
            var result = await controller.SelectAsync(id, CancellationToken.None);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should()
                .BeEquivalentTo(returns);
        }

        [Fact]
        public async Task MeetupControllerCreateAsyncCallsManager()
        {
            //Arrange
            managerMock.Setup(m => m.AddAsync(
                It.IsAny<MeetupViewModel>(), CancellationToken.None))
                    .Verifiable();

            //Act
            var result = await controller.CreateAsync(
                It.IsAny<MeetupViewModel>(), CancellationToken.None);

            //Assert
            managerMock.Verify();
        }

        [Fact]
        public async Task MeetupControllerUpdateAsyncCallsManager()
        {
            //Arrange
            managerMock.Setup(m => m.GetAsync(1, CancellationToken.None))
                .ReturnsAsync(GetTestMeetups().First());

            managerMock.Setup(m => m.UpdateAsync(
                1, It.IsAny<MeetupViewModel>(), CancellationToken.None))
                    .Verifiable();

            //Act
            var result = await controller.UpdateAsync(
                1, It.IsAny<MeetupViewModel>(), CancellationToken.None);

            //Assert
            managerMock.Verify();
        }

        [Fact]
        public async Task MeetupControllerDeleteAsyncCallsManager()
        {
            //Arrange
            managerMock.Setup(m => m.RemoveAsync(
                1, CancellationToken.None))
                    .Verifiable();

            //Act
            var result = await controller.DeleteAsync(
                1, CancellationToken.None);

            //Assert
            managerMock.Verify();
        }

        private List<Meetup> GetTestMeetups()
        {
            return new()
            {
                new()
                {
                    Id = 1,
                    Name = "Weather issue",
                    Description = "Test meeting",
                    Speaker = "John Doe",
                    DateTime = DateTime.Today,
                    Сountry = "USA",
                    State = "Ohio",
                    City = "Lima",
                    Street = "Linden St.",
                    House = "2B",
                    PostCode = "123456"
                },
                new()
                {
                    Id = 2,
                    Name = "Climate change",
                    Description = "Test meeting 2",
                    Speaker = "July Smith",
                    DateTime = DateTime.Today,
                    Сountry = "USA",
                    State = "Nevada",
                    City = "Las Vegas",
                    Street = "W Washington Ave",
                    House = "2101",
                    PostCode = "654321"
                }
            };
        }
    }
}