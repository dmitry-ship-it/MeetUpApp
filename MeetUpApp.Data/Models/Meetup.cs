namespace MeetUpApp.Data.Models
{
    public class Meetup
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Speaker { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public string Сountry { get; set; } = null!;

        public string State { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string House { get; set; } = null!;

        public string PostCode { get; set; } = null!;
    }
}
