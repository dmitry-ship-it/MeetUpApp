namespace MeetUpApp.ViewModels
{
    public class MeetupViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Speaker { get; set; }

        public DateTime DateTime { get; set; }

        public AddressViewModel Address { get; set; }
    }
}