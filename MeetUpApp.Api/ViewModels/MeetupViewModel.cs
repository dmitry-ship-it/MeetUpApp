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

        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public AddressViewModel Address { get; set; } = null!;
    }
}
