using AutoMapper;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using Moq;

namespace MeetUpApp.Managers.Tests
{
    public class MeetupManagerTests
    {
        [Fact]
        public async Task MeetupManagerGetAllAsyncCallsRepository()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Meetup>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(m => m.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(GetTestMeetups);
            var manager = new MeetupManager(mockRepository.Object, mockMapper.Object);

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
            var mockRepository = new Mock<IRepository<Meetup>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(m => m.GetByExpressionAsync(
                m => m.Id == id,CancellationToken.None))
                    .ReturnsAsync(GetTestMeetups()[0]);

            var manager = new MeetupManager(mockRepository.Object, mockMapper.Object);

            //Act
            var result = await manager.GetAsync(id);

            //Assert
            result.Should().BeEquivalentTo(GetTestMeetups().Single(m => m.Id == id));
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
