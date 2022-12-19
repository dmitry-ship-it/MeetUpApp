namespace MeetUpApp.Data.Models
{
    /// <summary>
    /// This model is used for auth.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Salt { get; set; } = null!;

        // good place for role prop
    }
}
