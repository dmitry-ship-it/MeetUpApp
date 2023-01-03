using MeetUpApp.Data.Models;

namespace MeetUpApp.Api.Tests
{
    internal static class TestData
    {
        public static List<Meetup> GetTestMeetups()
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
