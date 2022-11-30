using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetUpApp.Api.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        public DbSet<Meetup> Meetup { get; set; }

        public DbSet<User> User { get; set; }
    }
}
