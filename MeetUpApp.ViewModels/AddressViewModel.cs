using System.ComponentModel.DataAnnotations;

namespace MeetUpApp.ViewModels
{
    public class AddressViewModel
    {
        [Required]
        public string Сountry { get; set; }

        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string House { get; set; }

        [StringLength(6, MinimumLength = 6)]
        public string PostCode { get; set; }
    }
}