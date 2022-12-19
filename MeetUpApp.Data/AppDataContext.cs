using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetUpApp.Data
{
    public class AppDataContext : DbContext
    {
        public DbSet<Meetup> Meetup { get; set; }
        public DbSet<User> User { get; set; }

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // meetup
            BuildMeetupTable(modelBuilder);

            // user
            BuildUserTable(modelBuilder);
        }

        private static void BuildMeetupTable(ModelBuilder modelBuilder)
        {
            // id column
            modelBuilder.Entity<Meetup>()
                .HasKey(m => m.Id)
                .IsClustered();

            modelBuilder.Entity<Meetup>()
                .Property(m => m.Id)
                .UseIdentityColumn()
                .IsRequired();

            // name column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.Name)
                .IsUnicode()
                .IsRequired();

            // description column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.Description)
                .IsUnicode();

            // speaker column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.Speaker)
                .IsUnicode()
                .IsRequired();

            // datetime column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.DateTime)
                .IsRequired();

            // country column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.Сountry)
                .IsUnicode()
                .IsRequired();

            // state column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.State)
                .IsUnicode()
                .IsRequired(false);

            // city column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.City)
                .IsUnicode()
                .IsRequired();

            // street column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.Street)
                .IsUnicode()
                .IsRequired();

            // house column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.House)
                .IsUnicode()
                .HasMaxLength(20)
                .IsRequired();

            // postcode column
            modelBuilder.Entity<Meetup>()
                .Property(m => m.PostCode)
                .HasMaxLength(6)
                .IsFixedLength()
                .IsRequired(false);
        }

        private static void BuildUserTable(ModelBuilder modelBuilder)
        {
            // id column
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id)
                .IsClustered();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .UseIdentityColumn()
                .IsRequired();

            // name column
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsUnicode()
                .HasMaxLength(20)
                .IsRequired();

            // passwordhash column
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasMaxLength(88)
                .IsFixedLength()
                .IsRequired();

            // salt column
            modelBuilder.Entity<User>()
                .Property(u => u.Salt)
                .HasMaxLength(44)
                .IsFixedLength()
                .IsRequired();
        }
    }
}
