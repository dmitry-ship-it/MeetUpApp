using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetUpApp.Api.Data.Models
{
    /// <summary>
    /// This model is used for auth.
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [Column(TypeName = "nchar(44)")]
        public string Salt { get; set; } = null!;

        // good place for role prop
    }
}
