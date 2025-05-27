using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Entities;
using Remitee_Backend.Core.Repositories.Interfaces;
using Remitee_Backend.Core.Support.Paginator;
using Remitee_Backend.Data;
using System.Linq.Expressions;
using static Remitee_Backend.Core.Support.Extensions.QueryableExtension;

namespace Remitee_Backend.Core.Repositories
{
    public sealed class BookRepository : IBookRepository
    {
        private readonly DataContext _dataContext;

        public BookRepository(DataContext dataContext)
        {
            _dataContext = Guard.Against.Null(dataContext);
        }

        public async Task<Book?> GetAsync(Guid id)
        {
            return await _dataContext.Books.FindAsync(id);
        }

        public async Task<QueryResult<Book>> GetAllAsync(PagedQuery<GetPagedBooksDTO> parameters)
        {
            Expression<Func<Book, bool>> where = book => parameters.Filter == null ||
            ((string.IsNullOrEmpty(parameters.Filter.Author) || book.Title.Contains(parameters.Filter.Author)) &&
            (string.IsNullOrEmpty(parameters.Filter.Title) || book.Title.Contains(parameters.Filter.Title)) &&
            (string.IsNullOrEmpty(parameters.Filter.Description) || book.Title.Contains(parameters.Filter.Description)));

            var count = await _dataContext.Books
                .Where(where)
                .CountAsync();

            // This query could be AsNoTracking() but it's not gonna be for this example.
            var items = await _dataContext.Books
                .Where(where)
                .OrderBy($"{parameters.SortField ?? "Title"} {parameters.SortOrder}")
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new QueryResult<Book>(items, count);
        }

        public async Task<Book> InsertAsync(Book entity)
        {
            var book = await _dataContext.Books.AddAsync(entity);
            return book.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            // Could be a logical delete.
            await _dataContext.Books
                .Where(b => b.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
