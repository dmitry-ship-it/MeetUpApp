using MeetUpApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetUpApp.Api.Data.DAL
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDataContext _context;

        public UserRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.User.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.User.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        public async Task InsertAsync(User user, CancellationToken cancellationToken)
        {
            await _context.User.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(User user, CancellationToken cancellationToken)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Meetup.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
