using AutoMapper;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.ViewModels;
using Moq;

namespace MeetUpApp.Managers.Tests
{
    public class MeetupManagerTests
    {
        private readonly Mock<IRepository<Meetup>> mockRepository;
        private readonly MeetupManager manager;

        public MeetupManagerTests()
        {
            mockRepository = new Mock<IRepository<Meetup>>();
            manager = new MeetupManager(mockRepository.Object, Mock.Of<IMapper>());
        }

        [Fact]
        public async Task GetAllAsync_WhenMeetupsExist_ShouldReturnAllMeetups()
        {
            //Arrange
            var ct = CancellationToken.None;
            var expected = TestData.GetTestMeetups();

            mockRepository.Setup(m => m.GetAllAsync(ct))
                .ReturnsAsync(TestData.GetTestMeetups);

            //Act
            var result = await manager.GetAllAsync(ct);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAsync_WhenMeetupExists_ShouldReturnThisMeetup()
        {
            //Arrange
            const int id = 1;
            var ct = CancellationToken.None;
            var expected = TestData.GetTestMeetups()
                .Single(m => m.Id == id);

            mockRepository.Setup(m => m.GetByExpressionAsync(
                m => m.Id == id,ct))
                    .ReturnsAsync(expected);

            //Act
            var result = await manager.GetAsync(id, ct);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateAsync_WhenMeetupExists_ShouldCallRepository()
        {
            //Arrange
            const int id = 1;
            var ct = CancellationToken.None;
            var viewModel = new MeetupViewModel();

            mockRepository.Setup(m => m.GetByExpressionAsync(
                x => x.Id == id, ct))
                    .ReturnsAsync(TestData.GetTestMeetups().First());

            mockRepository.Setup(m => m.UpdateAsync(
                It.IsAny<Meetup>(), ct))
                    .Verifiable();

            //Act
            await manager.UpdateAsync(id, viewModel, ct);

            //Assert
            mockRepository.Verify();
        }

        [Fact]
        public async Task RemoveAsync_WhenMeetupExists_ShouldCallRepository()
        {
            //Arrange
            const int id = 1;
            var ct = CancellationToken.None;

            mockRepository.Setup(m => m.GetByExpressionAsync(
                x => x.Id == id, ct))
                    .ReturnsAsync(TestData.GetTestMeetups().First());

            mockRepository.Setup(m => m.RemoveAsync(
                It.IsAny<Meetup>(), ct))
                    .Verifiable();

            //Act
            await manager.RemoveAsync(id, ct);

            //Assert
            mockRepository.Verify();
        }

        [Fact]
        public async Task AddAsync_WhenCalling_ShouldCallRepository()
        {
            //Arrange
            var ct = CancellationToken.None;
            var viewModel = new MeetupViewModel();

            mockRepository.Setup(m => m.InsertAsync(
                It.IsAny<Meetup>(), ct))
                    .Verifiable();

            //Act
            await manager.AddAsync(viewModel, ct);

            //Assert
            mockRepository.Verify();
        }
    }
}
