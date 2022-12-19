namespace MeetUpApp.Data.Models
{
    /// <summary>
    /// This model is used for auth.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        // good place for role prop
    }
}
