using System.ComponentModel.DataAnnotations;

namespace MeetUpApp.Api.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
