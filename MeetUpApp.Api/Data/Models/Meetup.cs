using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetUpApp.Api.Data.Models
{
    public class Meetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; } = null!;
    }
}
