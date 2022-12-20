using System.ComponentModel.DataAnnotations;

namespace MeetUpApp.ViewModels
{
    public class MeetupViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Speaker { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public AddressViewModel Address { get; set; }
    }
}