using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MeetUpApp.Api.Data.Models
{
    public class Address
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Сountry { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string State { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string City { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Street { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string House { get; set; } = null!;

        [StringLength(6)]
        [Column(TypeName = "nchar(6)")]
        public string PostCode { get; set; } = null!;

        public virtual ICollection<Meetup> Meetups { get; set; } = null!;
    }
}
