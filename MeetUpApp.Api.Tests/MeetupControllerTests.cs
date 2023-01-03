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
        public async Task AllAsync_WhenMeetupsExist_ShouldReturnOkObjectResultWithMeetupsInBody()
        {
            //Arrange
            var ct = CancellationToken.None;
            var expected = new OkObjectResult(TestData.GetTestMeetups());

            managerMock.Setup(m => m.GetAllAsync(ct))
                .ReturnsAsync(TestData.GetTestMeetups);

            //Act
            var result = await controller.AllAsync(ct);

            //Assert
            result.Should().BeOfType<OkObjectResult>()
                .And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SelectAsync_WhenMeetupExists_ShouldReturnOkObjectResultWithMeetupInBody()
        {
            //Arrange
            const int id = 1;
            var ct = CancellationToken.None;
            var expected = new OkObjectResult(
                TestData.GetTestMeetups().Single(m => m.Id == id));

            var returns = TestData.GetTestMeetups().Single(m => m.Id == id);
            managerMock.Setup(m => m.GetAsync(id, ct))
                .ReturnsAsync(returns);

            //Act
            var result = await controller.SelectAsync(id, ct);

            //Assert
            result.Should().BeOfType<OkObjectResult>()
                .And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateAsync_WhenCalling_ShouldCallManager()
        {
            //Arrange
            var viewModel = It.IsAny<MeetupViewModel>();
            var ct = CancellationToken.None;

            managerMock.Setup(m => m.AddAsync(viewModel, ct))
                .Verifiable();

            //Act
            await controller.CreateAsync(viewModel, ct);

            //Assert
            managerMock.Verify();
        }

        [Fact]
        public async Task UpdateAsync_WhenMeetupExists_ShouldCallManager()
        {
            //Arrange
            const int id = 1;
            var viewModel = It.IsAny<MeetupViewModel>();
            var ct = CancellationToken.None;

            managerMock.Setup(m => m.GetAsync(id, ct))
                .ReturnsAsync(TestData.GetTestMeetups().First());

            managerMock.Setup(m => m.UpdateAsync(id, viewModel, ct))
                .Verifiable();

            //Act
            await controller.UpdateAsync(id, viewModel, ct);

            //Assert
            managerMock.Verify();
        }

        [Fact]
        public async Task DeleteAsync_WhenCalling_ShouldCallManager()
        {
            //Arrange
            const int id = 1;
            var ct = CancellationToken.None;

            managerMock.Setup(m => m.RemoveAsync(id, ct))
                .Verifiable();

            //Act
            await controller.DeleteAsync(id, ct);

            //Assert
            managerMock.Verify();
        }
    }
}
