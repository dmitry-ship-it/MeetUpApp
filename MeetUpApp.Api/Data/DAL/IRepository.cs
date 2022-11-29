using System.Linq.Expressions;

namespace MeetUpApp.Api.Data.DAL
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task InsertAsync(T obj, CancellationToken cancellationToken);

        Task UpdateAsync(T obj, CancellationToken cancellationToken);

        Task RemoveAsync(T obj, CancellationToken cancellationToken);
    }
}
