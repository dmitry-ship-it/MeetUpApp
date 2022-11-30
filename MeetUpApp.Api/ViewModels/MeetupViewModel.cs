using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MeetUpApp.Api.ViewModels
{
    public class MeetupViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Speaker { get; set; } = null!;

        [Required]
        public DateTime DateTime { get; set; }

        public AddressViewModel Address { get; set; } = null!;
    }
}
