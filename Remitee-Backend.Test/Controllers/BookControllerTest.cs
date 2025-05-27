using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Remitee_Backend.Core.Controllers;
using Remitee_Backend.Core.DataTransferObjects;
using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Services.Interfaces;
using Remitee_Backend.Core.Support.Paginator;

namespace Remitee_Backend.Core.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _controller;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BookController(_mockBookService.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenBookServiceIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BookController(null));
        }

        public class GetBooks : BooksControllerTests
        {
            [Fact]
            public async Task GetBooks_ReturnsOk_WhenBooksExist()
            {
                // Arrange
                var parameters = new PagedQuery<GetPagedBooksDTO>();
                var expectedResult = new PagedQueryResult<BookDTO>(new List<BookDTO> { new BookDTO() }, 1, 1, 10);

                _mockBookService.Setup(x => x.GetAllAsync(parameters))
                    .ReturnsAsync(expectedResult);

                // Act
                var result = await _controller.GetBooks(parameters);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var model = Assert.IsType<PagedQueryResult<BookDTO>>(okResult.Value);
                Assert.Equal(expectedResult, model);
            }

            [Fact]
            public async Task GetBooks_ReturnsNotFound_WhenNoBooksExist()
            {
                // Arrange
                var parameters = new PagedQuery<GetPagedBooksDTO>();
                var emptyResult = new PagedQueryResult<BookDTO>(new List<BookDTO>(), 0, 1, 10);

                _mockBookService.Setup(x => x.GetAllAsync(parameters))
                    .ReturnsAsync(emptyResult);

                // Act
                var result = await _controller.GetBooks(parameters);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class GetBook : BooksControllerTests
        {
            [Fact]
            public async Task GetBook_ReturnsOk_WhenBookExists()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                var expectedBook = new BookDTO { Id = bookId };

                _mockBookService.Setup(x => x.GetAsync(bookId))
                    .ReturnsAsync(expectedBook);

                // Act
                var result = await _controller.GetBook(bookId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var model = Assert.IsType<BookDTO>(okResult.Value);
                Assert.Equal(expectedBook, model);
            }
        }

        public class CreateBook : BooksControllerTests
        {
            [Fact]
            public async Task CreateBook_ReturnsCreatedAtAction_WhenSuccessful()
            {
                // Arrange
                var newBookDto = new CreateBookDTO()
                {
                    Author = "Author Name",
                    Title = "Book Title",
                    Description = "Book Description"
                };
                var createdBook = new BookDTO { Id = Guid.NewGuid() };

                _mockBookService.Setup(x => x.CreateAsync(newBookDto))
                    .ReturnsAsync(createdBook);

                // Act
                var result = await _controller.CreateBook(newBookDto);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal(nameof(BookController.GetBook), createdAtActionResult.ActionName);
                Assert.Equal(createdBook.Id, ((BookDTO)createdAtActionResult.Value).Id);
                Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
            }
        }

        public class DeleteBook : BooksControllerTests
        {
            [Fact]
            public async Task DeleteBook_ReturnsNoContent_WhenSuccessful()
            {
                // Arrange
                var bookId = Guid.NewGuid();

                _mockBookService.Setup(x => x.DeleteAsync(bookId))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.DeleteBook(bookId);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }

        public class UpdateBook : BooksControllerTests
        {
            [Fact]
            public async Task UpdateBook_ReturnsNoContent_WhenSuccessful()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                var updateDto = new UpdateBookDTO()
                {
                    Author = "Author Name",
                    Title = "Updated Title",
                    Description = "Updated Description",
                };

                _mockBookService.Setup(x => x.UpdateAsync(bookId, updateDto))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.UpdateBook(bookId, updateDto);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }
    }
}