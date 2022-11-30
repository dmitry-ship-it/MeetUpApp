using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeetUpApp.Api.Data.DAL
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDataContext _context;

        public UserRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.User.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.User.SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.User.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        public async Task InsertAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.User.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
