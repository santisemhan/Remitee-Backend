using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Entities;

namespace Remitee_Backend.Core.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book, GetPagedBooksDTO> { }
}
