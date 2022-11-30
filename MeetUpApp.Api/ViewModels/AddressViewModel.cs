using System.ComponentModel.DataAnnotations;

namespace MeetUpApp.Api.ViewModels
{
    public class AddressViewModel
    {
        [Required]
        public string Сountry { get; set; } = null!;

        public string State { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Street { get; set; } = null!;

        [Required]
        public string House { get; set; } = null!;

        [StringLength(6, MinimumLength = 6)]
        public string PostCode { get; set; } = null!;
    }
}
