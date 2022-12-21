using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetUpApp.Data.EntityConfiguration
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // id column
            builder.HasKey(u => u.Id)
                .IsClustered();

            builder.Property(u => u.Id)
                .UseIdentityColumn()
                .IsRequired();

            // name column
            builder.HasIndex(u => u.Name)
                .IsUnique();

            builder.Property(u => u.Name)
                .HasMaxLength(20)
                .IsUnicode()
                .IsRequired();

            // passwordhash column
            builder.Property(u => u.PasswordHash)
                .HasMaxLength(88)
                .IsFixedLength()
                .IsRequired();

            // salt column
            builder.Property(u => u.Salt)
                .HasMaxLength(44)
                .IsFixedLength()
                .IsRequired();
        }
    }
}
