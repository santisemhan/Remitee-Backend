namespace Remitee_Backend.Core.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBookRepository Books { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
