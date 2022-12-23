using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetUpApp.Data.EntityConfiguration
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id)
                .IsClustered();

            builder.Property(u => u.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.HasIndex(u => u.Name)
                .IsUnique();

            builder.Property(u => u.Name)
                .HasMaxLength(20)
                .IsUnicode()
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(88)
                .IsFixedLength()
                .IsRequired();

            builder.Property(u => u.Salt)
                .HasMaxLength(44)
                .IsFixedLength()
                .IsRequired();
        }
    }
}
