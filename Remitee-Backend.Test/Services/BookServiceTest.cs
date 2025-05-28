using Moq;
using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Entities;
using Remitee_Backend.Core.Repositories.Interfaces;
using Remitee_Backend.Core.Services;
using Remitee_Backend.Core.Support.Paginator;

namespace Remitee_Backend.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBookRepository = new Mock<IBookRepository>();

            _mockUnitOfWork.Setup(u => u.Books).Returns(_mockBookRepository.Object);

            _bookService = new BookService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenUnitOfWorkIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BookService(null));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenBookRepositoryIsNull()
        {
            // Arrange
            var mockInvalidUnitOfWork = new Mock<IUnitOfWork>();
            mockInvalidUnitOfWork.Setup(u => u.Books).Returns((IBookRepository)null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BookService(mockInvalidUnitOfWork.Object));
        }

        public class CreateAsync : BookServiceTests
        {
            [Fact]
            public async Task CreateAsync_ShouldReturnBookDTO_WhenSuccessful()
            {
                // Arrange
                var createBookDto = new CreateBookDTO
                {
                    Author = "Test Author",
                    Title = "Test Title",
                    Description = "Test Description"
                };

                var expectedBook = new Book
                {
                    Id = Guid.NewGuid(),
                    Author = createBookDto.Author,
                    Title = createBookDto.Title,
                    Description = createBookDto.Description
                };

                _mockBookRepository.Setup(r => r.InsertAsync(It.IsAny<Book>()))
                    .ReturnsAsync(expectedBook);

                // Act
                var result = await _bookService.CreateAsync(createBookDto);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedBook.Id, result.Id);
                Assert.Equal(expectedBook.Author, result.Author);
                Assert.Equal(expectedBook.Title, result.Title);
                Assert.Equal(expectedBook.Description, result.Description);

                _mockBookRepository.Verify(r => r.InsertAsync(It.IsAny<Book>()), Times.Once);
                _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        public class GetAsync : BookServiceTests
        {
            [Fact]
            public async Task GetAsync_ShouldReturnBookDTO_WhenBookExists()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                var expectedBook = new Book
                {
                    Id = bookId,
                    Author = "Existing Author",
                    Title = "Existing Title",
                    Description = "Existing Description"
                };

                _mockBookRepository.Setup(r => r.GetAsync(bookId))
                    .ReturnsAsync(expectedBook);

                // Act
                var result = await _bookService.GetAsync(bookId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedBook.Id, result.Id);
                Assert.Equal(expectedBook.Author, result.Author);
                Assert.Equal(expectedBook.Title, result.Title);
                Assert.Equal(expectedBook.Description, result.Description);
            }

            [Fact]
            public async Task GetAsync_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                _mockBookRepository.Setup(r => r.GetAsync(bookId))
                    .ReturnsAsync((Book)null);

                // Act & Assert
                await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookService.GetAsync(bookId));
            }
        }

        public class GetAllAsync : BookServiceTests
        {
            [Fact]
            public async Task GetAllAsync_ShouldReturnPagedBooks_WhenBooksExist()
            {
                // Arrange
                var parameters = new PagedQuery<GetPagedBooksDTO>
                {
                    PageNumber = 1,
                    PageSize = 10
                };

                var books = new List<Book>
                {
                    new Book { Id = Guid.NewGuid(), Author = "Author 1", Title = "Title 1", Description = "Description" },
                    new Book { Id = Guid.NewGuid(), Author = "Author 2", Title = "Title 2", Description = "Description" }
                };

                var queryResult = new QueryResult<Book>(books, 2);

                _mockBookRepository.Setup(r => r.GetAllAsync(parameters))
                    .ReturnsAsync(queryResult);

                // Act
                var result = await _bookService.GetAllAsync(parameters);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.TotalItems);
                Assert.Equal(2, result.Items.Count());
                Assert.Equal("Author 1", result.Items.First().Author);
                Assert.Equal("Title 2", result.Items.Last().Title);
            }

            [Fact]
            public async Task GetAllAsync_ShouldReturnEmptyPagedResult_WhenNoBooksExist()
            {
                // Arrange
                var parameters = new PagedQuery<GetPagedBooksDTO>();
                var emptyQueryResult = new QueryResult<Book>(new List<Book>(), 0);

                _mockBookRepository.Setup(r => r.GetAllAsync(parameters))
                    .ReturnsAsync(emptyQueryResult);

                // Act
                var result = await _bookService.GetAllAsync(parameters);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(0, result.TotalItems);
                Assert.Empty(result.Items);
            }
        }

        public class DeleteAsync : BookServiceTests
        {
            [Fact]
            public async Task DeleteAsync_ShouldDeleteBook_WhenBookExists()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                var existingBook = new Book { Id = bookId, Author = "Author", Description = "Description", Title = "Title" };

                _mockBookRepository.Setup(r => r.GetAsync(bookId))
                    .ReturnsAsync(existingBook);

                // Act
                await _bookService.DeleteAsync(bookId);

                // Assert
                _mockBookRepository.Verify(r => r.DeleteAsync(bookId), Times.Once);
                _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                _mockBookRepository.Setup(r => r.GetAsync(bookId))
                    .ReturnsAsync((Book)null);

                // Act & Assert
                await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookService.DeleteAsync(bookId));
                _mockBookRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
                _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        public class UpdateAsync : BookServiceTests
        {
            [Fact]
            public async Task UpdateAsync_ShouldUpdateBook_WhenBookExists()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                var existingBook = new Book
                {
                    Id = bookId,
                    Author = "Old Author",
                    Title = "Old Title",
                    Description = "Old Description"
                };

                var updateDto = new UpdateBookDTO
                {
                    Author = "New Author",
                    Title = "New Title",
                    Description = "New Description"
                };

                _mockBookRepository.Setup(r => r.GetAsync(bookId))
                    .ReturnsAsync(existingBook);

                // Act
                await _bookService.UpdateAsync(bookId, updateDto);

                // Assert
                Assert.Equal(updateDto.Author, existingBook.Author);
                Assert.Equal(updateDto.Title, existingBook.Title);
                Assert.Equal(updateDto.Description, existingBook.Description);
                _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
            {
                // Arrange
                var bookId = Guid.NewGuid();
                var updateDto = new UpdateBookDTO() { Author = "Author", Description = "Description", Title = "Title"};

                _mockBookRepository.Setup(r => r.GetAsync(bookId))
                   .ReturnsAsync((Book)null);

                // Act & Assert
                await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookService.UpdateAsync(bookId, updateDto));
                _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            }
        }
    }
}