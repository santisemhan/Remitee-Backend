using Ardalis.GuardClauses;
using Remitee_Backend.Core.DataTransferObjects;
using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Entities;
using Remitee_Backend.Core.Repositories.Interfaces;
using Remitee_Backend.Core.Services.Interfaces;
using Remitee_Backend.Core.Support.Paginator;

namespace Remitee_Backend.Core.Services
{
    public sealed class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork) 
        { 
           _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
           _bookRepository = Guard.Against.Null(unitOfWork.Books, nameof(unitOfWork.Books));
        }

        public async Task<BookDTO> CreateAsync(CreateBookDTO newBook)
        {
            Book book = new()
            {
                Author = newBook.Author,
                Title = newBook.Title,
                Description = newBook.Description
            };
            var persistedBook = await _bookRepository.InsertAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return new BookDTO(persistedBook);
        }

        public async Task<BookDTO> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }
            return new BookDTO(book);
        }

        public async Task<PagedQueryResult<BookDTO>> GetAllAsync(PagedQuery<GetPagedBooksDTO> parameters)
        {
            var books = await _bookRepository.GetAllAsync(parameters);
            var mappedBooks = books.Items.Select(book => new BookDTO(book)).ToList();
            return new PagedQueryResult<BookDTO>(mappedBooks, books.TotalItems, parameters.PageNumber, parameters.PageSize);
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }
            await _bookRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateBookDTO updatedBook)
        {
            var book = await _bookRepository.GetAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            // Maybe I can create an Update method in the Book class to handle this
            book.Author = updatedBook.Author;
            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;

            // Not need to call repository's update method, as the book is already tracked by the context
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
