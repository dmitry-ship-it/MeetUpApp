using AutoMapper;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.ViewModels;
using MeetUpApp.ViewModels.Mapping;
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

            //var profile = new MeetupProfile();
            //var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            //IMapper mapper = new Mapper(configuration);

            manager = new MeetupManager(mockRepository.Object, Mock.Of<IMapper>());
        }

        [Fact]
        public async Task MeetupManagerGetAllAsyncCallsRepository()
        {
            //Arrange
            mockRepository.Setup(m => m.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(GetTestMeetups);

            //Act
            var result = await manager.GetAllAsync();

            //Assert
            result.Should().BeEquivalentTo(GetTestMeetups());
        }

        [Fact]
        public async Task MeetupManagerGetAsyncCallsRepository()
        {
            //Arrange
            const int id = 1;
            mockRepository.Setup(m => m.GetByExpressionAsync(
                m => m.Id == id,CancellationToken.None))
                    .ReturnsAsync(GetTestMeetups()[0]);

            //Act
            var result = await manager.GetAsync(id);

            //Assert
            result.Should().BeEquivalentTo(GetTestMeetups().Single(m => m.Id == id));
        }

        [Fact]
        public async Task MeetupManagerUpdateAsyncCallsRepository()
        {
            //Arrange
            const int id = 1;
            mockRepository.Setup(m => m.GetByExpressionAsync(
                x => x.Id == id, CancellationToken.None))
                    .ReturnsAsync(GetTestMeetups()[0]);

            mockRepository.Setup(m => m.UpdateAsync(
                It.IsAny<Meetup>(), CancellationToken.None))
                    .Verifiable();

            //Act
            await manager.UpdateAsync(
                1,
                new MeetupViewModel(),
                CancellationToken.None);

            //Assert
            mockRepository.Verify();
        }

        [Fact]
        public async Task MeetupManagerRemoveAsyncCallsRepository()
        {
            //Arrange
            const int id = 1;
            mockRepository.Setup(m => m.GetByExpressionAsync(
                x => x.Id == id, CancellationToken.None))
                    .ReturnsAsync(GetTestMeetups()[0]);

            mockRepository.Setup(m => m.RemoveAsync(
                It.IsAny<Meetup>(), CancellationToken.None))
                    .Verifiable();

            //Act
            await manager.RemoveAsync(1, CancellationToken.None);

            //Assert
            mockRepository.Verify();
        }

        [Fact]
        public async Task MeetupManagerAddAsyncCallsRepository()
        {
            //Arrange
            mockRepository.Setup(m => m.InsertAsync(
                It.IsAny<Meetup>(), CancellationToken.None))
                    .Verifiable();

            //Act
            await manager.AddAsync(
                new MeetupViewModel(),
                CancellationToken.None);

            //Assert
            mockRepository.Verify();
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
