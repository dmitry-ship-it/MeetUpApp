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

        [StringLength(6, MinimumLength = 6)]
        [Column(TypeName = "nchar(6)")]
        public string PostCode { get; set; } = null!;

    }
}
