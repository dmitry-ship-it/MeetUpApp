using System.Linq.Expressions;

namespace MeetUpApp.Data.DAL
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);

        Task InsertAsync(T obj, CancellationToken cancellationToken = default);

        Task UpdateAsync(T obj, CancellationToken cancellationToken = default);

        Task RemoveAsync(T obj, CancellationToken cancellationToken = default);
    }
}
