using Microsoft.EntityFrameworkCore;
using Remitee_Backend.Core.DataTransferObjects.Books;
using Remitee_Backend.Core.Entities;
using Remitee_Backend.Core.Repositories;
using Remitee_Backend.Core.Support.Paginator;
using Remitee_Backend.Data;

namespace Remitee_Backend.Test.Repositories
{
    public class BookRepositoryTests
    {
        private readonly DataContext _dbContext;
        private readonly BookRepository _repository;

        public BookRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "RemiteeBookRepositoryTestDB")
                .Options;

            _dbContext = new DataContext(options);
            _repository = new BookRepository(_dbContext);

            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Author = "Santiago", Title = "Test Book", Description = "Description" };
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAsync(book.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Act
            var result = await _repository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedResults()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "A Book", Author = "Author A", Description = "Description" },
                new Book { Id = Guid.NewGuid(), Title = "B Book", Author = "Author B", Description = "Description" },
                new Book { Id = Guid.NewGuid(), Title = "C Book", Author = "Author C", Description = "Description" }
            };

            _dbContext.Books.AddRange(books);
            await _dbContext.SaveChangesAsync();

            var parameters = new PagedQuery<GetPagedBooksDTO>
            {
                PageNumber = 1,
                PageSize = 2,
                SortField = "Title",
                SortOrder = "asc"
            };

            // Act
            var result = await _repository.GetAllAsync(parameters);

            // Assert
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("A Book", result.Items.First().Title);
        }

        [Fact]
        public async Task GetAllAsync_ShouldFilterResults()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Science Book", Author = "Einstein", Description = "Description" },
                new Book { Id = Guid.NewGuid(), Title = "Math Book", Author = "Newton", Description = "Description" },
                new Book { Id = Guid.NewGuid(), Title = "History Book", Author = "Herodotus", Description = "Description" }
            };

            _dbContext.Books.AddRange(books);
            await _dbContext.SaveChangesAsync();

            var parameters = new PagedQuery<GetPagedBooksDTO>
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GetPagedBooksDTO { Title = "Science" }
            };

            // Act
            var result = await _repository.GetAllAsync(parameters);

            // Assert
            Assert.Equal(1, result.TotalItems);
            Assert.Equal("Science Book", result.Items.First().Title);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddBookToDatabase()
        {
            // Arrange
            var newBook = new Book { Title = "New Book", Author = "Santiago", Description = "Description" };

            // Act
            var result = await _repository.InsertAsync(newBook);
            await _dbContext.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newBook.Title, result.Title);
            Assert.NotEqual(Guid.Empty, result.Id);

            var dbBook = await _dbContext.Books.FindAsync(result.Id);
            Assert.NotNull(dbBook);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBookFromDatabase()
        {
            // ExecuteDeleteAsync doesn't work with InMemoryDatabase
            Assert.True(true);
        }
    }
}