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
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDataContext).Assembly);
        }
    }
}