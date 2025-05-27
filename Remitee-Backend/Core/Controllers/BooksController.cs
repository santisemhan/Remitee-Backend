using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Remitee_Backend.Core.DataTransferObjects;
using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Services.Interfaces;
using Remitee_Backend.Core.Support.Paginator;

namespace Remitee_Backend.Core.Controllers
{
    [ApiController]
    [Route("api/books")]
    [Produces("application/json")]
    public sealed class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = Guard.Against.Null(bookService, nameof(bookService));
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>A list of all available books.</returns>
        /// <response code="200">Returns the list of books successfully.</response>
        /// <response code="404">No books were found.</response>
        /// <response code="500">Internal server error while processing the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedQueryResult<BookDTO>))]
        public async Task<IActionResult> GetBooks([FromQuery] PagedQuery<GetPagedBooksDTO> parameters)
        {
            var books = await _bookService.GetAllAsync(parameters);
            if (books.TotalItems == 0)
            {
                return NotFound();
            }

            return Ok(books);
        }

        /// <summary>
        /// Retrieves a specific book by its unique identifier.
        /// </summary>
        /// <param name="id">The GUID of the book to retrieve</param>
        /// <returns>The requested book details</returns>
        /// <response code="200">Returns the requested book</response>
        /// <response code="404">If no book exists with the specified ID</response>
        /// <response code="400">If the provided ID is not a valid GUID</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBook([FromRoute] Guid id)
        {
            var book = await _bookService.GetAsync(id);
            return Ok(book);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="newBookDTO">Data to create a new book</param>
        /// <returns>The newly created book details.</returns>
        /// <response code="201">Returns the created book with its unique identifier.</response>
        /// <response code="400">Invalid request data or validation failed.</response>
        /// <response code="500">Internal server error while creating the book.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO newBookDTO)
        {
            var newBook = await _bookService.CreateAsync(newBookDTO);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        /// <summary>
        /// Deletes a specific book by its unique identifier.
        /// </summary>
        /// <param name="id">The GUID of the book to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the book was deleted successfully</response>
        /// <response code="404">If no book exists with the specified ID</response>
        /// <response code="400">If the provided ID is not valid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The GUID of the book to update</param>
        /// <param name="updatedBookDTO">Data to update the book</param>
        /// <returns>The updated book details</returns>
        /// <response code="200">Returns the updated book</response>
        /// <response code="404">If no book exists with the specified ID</response>
        /// <response code="400">If the provided ID is not valid or request data is invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookDTO updatedBookDTO)
        {
            await _bookService.UpdateAsync(id, updatedBookDTO);
            return NoContent();
        }
    }
}
