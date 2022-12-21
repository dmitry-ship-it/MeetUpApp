using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetUpApp.Data.EntityConfiguration
{
    internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
    {
        public void Configure(EntityTypeBuilder<Meetup> builder)
        {
            // id column
            builder.HasKey(m => m.Id)
                .IsClustered();

            builder.Property(m => m.Id)
                .UseIdentityColumn()
                .IsRequired();

            // name column
            builder.Property(m => m.Name)
                .IsUnicode()
                .IsRequired();

            // description column
            builder.Property(m => m.Description)
                .IsUnicode();

            // speaker column
            builder.Property(m => m.Speaker)
                .IsUnicode()
                .IsRequired();

            // datetime column
            builder.Property(m => m.DateTime)
                .IsRequired();

            // country column
            builder.Property(m => m.Сountry)
                .IsUnicode()
                .IsRequired();

            // state column
            builder.Property(m => m.State)
                .IsUnicode()
                .IsRequired(false);

            // city column
            builder.Property(m => m.City)
                .IsUnicode()
                .IsRequired();

            // street column
            builder.Property(m => m.Street)
                .IsUnicode()
                .IsRequired();

            // house column
            builder.Property(m => m.House)
                .IsUnicode()
                .HasMaxLength(20)
                .IsRequired();

            // postcode column
            builder.Property(m => m.PostCode)
                .HasMaxLength(6)
                .IsFixedLength()
                .IsRequired(false);
        }
    }
}
