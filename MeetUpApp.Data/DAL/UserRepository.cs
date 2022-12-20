using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeetUpApp.Data.DAL
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDataContext context;

        public UserRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await context.User.AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<User> GetByExpressionAsync(
            Expression<Func<User, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await context.User.AsNoTracking()
                .SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task InsertAsync(User user,
            CancellationToken cancellationToken = default)
        {
            await context.User.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(User user,
            CancellationToken cancellationToken = default)
        {
            context.User.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user,
            CancellationToken cancellationToken = default)
        {
            context.User.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}