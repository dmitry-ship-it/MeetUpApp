using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetUpApp.Api.Data.DAL
{
    public class MeetupRepository : IRepository<Meetup>
    {
        private readonly AppDataContext _context;

        public MeetupRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meetup>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Meetup.ToListAsync(cancellationToken);
        }

        public async Task<Meetup?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Meetup.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        public async Task InsertAsync(Meetup meetup, CancellationToken cancellationToken)
        {
            await _context.Meetup.AddAsync(meetup, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Meetup meetup, CancellationToken cancellationToken)
        {
            _context.Meetup.Remove(meetup);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Meetup meetup, CancellationToken cancellationToken)
        {
            _context.Meetup.Update(meetup);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
