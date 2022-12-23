using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetUpApp.Data.EntityConfiguration
{
    internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
    {
        public void Configure(EntityTypeBuilder<Meetup> builder)
        {
            builder.HasKey(m => m.Id)
                .IsClustered();

            builder.Property(m => m.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(m => m.Name)
                .IsUnicode()
                .IsRequired();

            builder.Property(m => m.Description)
                .IsUnicode();

            builder.Property(m => m.Speaker)
                .IsUnicode()
                .IsRequired();

            builder.Property(m => m.DateTime)
                .IsRequired();

            builder.Property(m => m.Сountry)
                .IsUnicode()
                .IsRequired();

            builder.Property(m => m.State)
                .IsUnicode()
                .IsRequired(false);

            builder.Property(m => m.City)
                .IsUnicode()
                .IsRequired();

            builder.Property(m => m.Street)
                .IsUnicode()
                .IsRequired();

            builder.Property(m => m.House)
                .IsUnicode()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(m => m.PostCode)
                .HasMaxLength(6)
                .IsFixedLength()
                .IsRequired(false);
        }
    }
}
