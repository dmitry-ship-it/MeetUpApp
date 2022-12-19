using System.ComponentModel.DataAnnotations;

namespace MeetUpApp.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
