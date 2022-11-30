using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeetUpApp.Api.Data.DAL
{
    public class MeetupRepository : IRepository<Meetup>
    {
        private readonly AppDataContext _context;

        public MeetupRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meetup>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Meetup.ToListAsync(cancellationToken);
        }

        public async Task<Meetup?> GetByExpressionAsync(Expression<Func<Meetup, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Meetup
                .SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<Meetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Meetup.FindAsync(
                new object[] { id }, cancellationToken);
        }

        public async Task InsertAsync(Meetup meetup, CancellationToken cancellationToken = default)
        {
            await _context.Meetup.AddAsync(meetup, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Meetup meetup, CancellationToken cancellationToken = default)
        {
            _context.Meetup.Remove(meetup);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Meetup meetup, CancellationToken cancellationToken = default)
        {
            _context.Meetup.Update(meetup);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
