using Remitee_Backend.Core.Models;
using Remitee_Backend.Core.Support.Paginator;

namespace Remitee_Backend.Core.Repositories.Interfaces
{
    /**
     * T represents the Entity type that owns the repository.
     * U represents the DTO type used as parameters for paged queries.
     */
    public interface IGenericRepository<T, U> where T : Entity where U : DataTransferObject
    {
        Task DeleteAsync(Guid guid);

        Task<T?> GetAsync(Guid id);

        Task<QueryResult<T>> GetAllAsync(PagedQuery<U> parameters);

        Task<T> InsertAsync(T entity);
    }
}
