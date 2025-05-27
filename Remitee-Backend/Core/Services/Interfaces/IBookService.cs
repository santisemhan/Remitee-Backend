using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Support.Paginator;

namespace Remitee_Backend.Core.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookDTO> CreateAsync(CreateBookDTO newBook);

        Task DeleteAsync(Guid id);

        Task<PagedQueryResult<BookDTO>> GetAllAsync(PagedQuery<GetPagedBooksDTO> parameters);

        Task<BookDTO> GetAsync(Guid id);

        Task UpdateAsync(Guid id, UpdateBookDTO updatedBook);
    }
}
