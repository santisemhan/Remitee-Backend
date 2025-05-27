using Ardalis.GuardClauses;
using Remitee_Backend.Core.Repositories.Interfaces;
using Remitee_Backend.Data;

namespace Remitee_Backend.Core.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IBookRepository Books { get; }

        public UnitOfWork(DataContext context, IBookRepository _bookRepository)
        {
            _context = Guard.Against.Null(context);
            Books = Guard.Against.Null(_bookRepository, nameof(_bookRepository));
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
