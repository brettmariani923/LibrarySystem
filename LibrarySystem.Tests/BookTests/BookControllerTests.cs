using LibrarySystem.API.Controllers;
using LibrarySystem.Application.DTO;
using LibrarySystem.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibrarySystem.Tests.BookTests
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly BooksController _controller;

        public BookControllerTests()
        {
            _mockService = new Mock<IBookService>();
            _controller = new BooksController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkWithBookList()
        {
            // Arrange
            var mockBooks = new List<BookDTO>
            {
                new BookDTO { Id = 1, Title = "Test Book 1", Author = "Author A" },
                new BookDTO { Id = 2, Title = "Test Book 2", Author = "Author B" }
            };

            _mockService.Setup(service => service.GetAllAsync())
                        .ReturnsAsync(mockBooks);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBooks = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(okResult.Value);
            Assert.Equal(2, ((List<BookDTO>)returnBooks).Count);
        }

        [Fact]
        public async Task GetAllBooks_EmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllAsync())
                        .ReturnsAsync(new List<BookDTO>());
            // Act
            var result = await _controller.GetAll();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBooks = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(okResult.Value);
            Assert.Empty(returnBooks);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkWithBook()
        {
            // Arrange
            var mockBook = new BookDTO { Id = 1, Title = "Test Book", Author = "Author A" };
            _mockService.Setup(service => service.GetByIdAsync(1))
                        .ReturnsAsync(mockBook);
            // Act
            var result = await _controller.GetById(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBook = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal("Test Book", returnBook.Title);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtActionWithBook()
        {
            // Arrange
            var newBook = new BookDTO { Title = "New Book", Author = "Author C" };
            var createdBook = new BookDTO { Id = 1, Title = "New Book", Author = "Author C" };
            _mockService.Setup(service => service.CreateAsync(newBook))
                        .ReturnsAsync(createdBook);
            // Act
            var result = await _controller.Create(newBook);
            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnBook = Assert.IsType<BookDTO>(createdResult.Value);
            Assert.Equal("New Book", returnBook.Title);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent()
        {
            // Arrange
            var updatedBook = new BookDTO { Id = 1, Title = "Updated Book", Author = "Author A" };
            _mockService.Setup(service => service.UpdateAsync(1, updatedBook))
                        .ReturnsAsync(true);
            // Act
            var result = await _controller.Update(1, updatedBook);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteAsync(1))
                        .ReturnsAsync(true);
            // Act
            var result = await _controller.Delete(1);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetBookById_NotFound_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetByIdAsync(1))
                        .ReturnsAsync((BookDTO?)null);
            // Act
            var result = await _controller.GetById(1);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateBook_NotFound_ReturnsNotFound()
        {
            // Arrange
            var updatedBook = new BookDTO { Id = 1, Title = "Updated Book", Author = "Author A" };
            _mockService.Setup(service => service.UpdateAsync(1, updatedBook))
                        .ReturnsAsync(false);
            // Act
            var result = await _controller.Update(1, updatedBook);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBook_NotFound_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteAsync(1))
                        .ReturnsAsync(false);
            // Act
            var result = await _controller.Delete(1);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateBook_NullBook_ReturnsBadRequest()
        {
            // Arrange
            BookDTO? nullBook = null;
            // Act
            var result = await _controller.Create(nullBook);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }

}
