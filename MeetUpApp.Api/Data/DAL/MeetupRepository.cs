using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeetUpApp.Api.Data.DAL
{
    public class MeetupRepository : IRepository<Meetup>
    {
        private readonly AppDataContext context;

        public MeetupRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Meetup>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Meetup.ToListAsync(cancellationToken);
        }

        public async Task<Meetup?> GetByExpressionAsync(Expression<Func<Meetup, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await context.Meetup
                .SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<Meetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Meetup.FindAsync(
                new object[] { id }, cancellationToken);
        }

        public async Task InsertAsync(Meetup meetup, CancellationToken cancellationToken = default)
        {
            await context.Meetup.AddAsync(meetup, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Meetup meetup, CancellationToken cancellationToken = default)
        {
            context.Meetup.Remove(meetup);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Meetup meetup, CancellationToken cancellationToken = default)
        {
            context.Meetup.Update(meetup);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
