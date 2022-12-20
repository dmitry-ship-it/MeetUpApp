namespace MeetUpApp.Data.Models
{
    public class Meetup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Speaker { get; set; }

        public DateTime DateTime { get; set; }

        public string Сountry { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string PostCode { get; set; }
    }
}