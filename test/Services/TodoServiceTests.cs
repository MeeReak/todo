using api.Dtos.Todo;
using api.Helper;
using api.Mappers;
using api.Models;
using api.Repository.Interface;
using api.Services;
using FluentAssertions;
using Moq;

namespace test.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _todoRepository;
        private readonly TodoService _todoService;

        public TodoServiceTests()
        {
            _todoRepository = new Mock<ITodoRepository>();
            _todoService = new TodoService(_todoRepository.Object);
        }


        [Fact]
        public async Task GetTodoAsync_ReturnsTodo_WhenTodoExists()
        {
            var expectedTodo = new Todo
            {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            };
            _todoRepository.Setup(x => x.GetTodoAsync(1)).ReturnsAsync(expectedTodo);

            var result = await _todoService.GetTodoAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Unit Test with C#");
        }

        [Fact]
        public async Task GetTodoAsync_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            //Arrange
            var todo = new Todo()
            {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            };
            _todoRepository.Setup(x => x.GetTodoAsync(1)).ReturnsAsync(todo);
            //Act
            var result = await _todoService.GetTodoAsync(2);

            //Assert
            result.Should().BeNull();

        }

        [Fact]
        public async Task GetTodosAsync_ReturnsTodos_WhenTodosExist()
        {
            //Arrange
            List<Todo> expectedTodo =
            [
               new Todo {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            },
               new Todo {
                Id = 2,
                Title = "Testing Code",
                Status = Status.InProgress,
                Description = "In Fact, Code is testing mee",
                IsDeleted = false
            }
            ];
            _todoRepository.Setup(x => x.GetTodosAsync()).ReturnsAsync(expectedTodo);

            //Act

            var result = await _todoService.GetTodosAsync();
            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedTodo.Select(TodoMapper.ToDto));
            result.Should().BeOfType<List<TodoDto>>();
        }

        [Fact]
        public async Task GetTodosAsync_CreatesNewTodo_WhenTodoExists()
        {
            //Arrange
            var todo = new Todo
            {
                Id = 1,
                Title = "Test Code",
                Status = Status.InProgress,
                Description = "Codes are testing mee",
                IsDeleted = false
            };
            var todoRequest = new TodoRequest
            {
                Title = "Test Code",
                Status = Status.InProgress,
                Description = "Codes are testing mee"
            };
            _todoRepository.Setup(x => x.AddTodoAsync(It.IsAny<Todo>()))
    .ReturnsAsync((Todo t) => t); // Returns the same instance passed in

            //Act
            var result = await _todoService.AddTodoAsync(todoRequest);
            //Assert
            result.Should().NotBeNull();
        }
       
        [Fact]
        public async Task GetTodoAsync_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            _todoRepository.Setup(x => x.GetTodoAsync(It.IsAny<int>()))
                           .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act
            var act = async () => await _todoService.GetTodoAsync(1);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                    .WithMessage("Database error");
        }

        [Fact]
        public async Task AddTodoAsync_ThrowsException_WhenTitleIsEmpty()
        {
            //arrange
            var todo = new TodoRequest
            {
                Title = "",
                Status = Status.InProgress,
                Description = "Old Description",
            };
            _todoRepository.Setup(x => x.AddTodoAsync(It.IsAny<Todo>())).ReturnsAsync(It.IsAny<Todo>());

            //act & assert
            await FluentActions
         .Awaiting(() => _todoService.AddTodoAsync(todo))
         .Should()
         .ThrowAsync<ArgumentException>()
         .WithMessage("Title is required");
        }

        [Fact]
        public async Task UpdateTodoAsync_ReturnsUpdatedTodo_WhenTodoExists()
        {
            // Arrange: existing todo and update request
            var existingTodo = new Todo
            {
                Id = 1,
                Title = "Old Title",
                Status = Status.InProgress,
                Description = "Old Description",
                IsDeleted = false
            };

            var updateRequest = new TodoRequest
            {
                Title = "New Title",
                Status = Status.Done,
                Description = "New Description"
            };

            var updatedTodo = new Todo
            {
                Id = 1,
                Title = updateRequest.Title,
                Status = updateRequest.Status,
                Description = updateRequest.Description,
                IsDeleted = false
            };

            // Setup repository mock for update
            _todoRepository.Setup(x => x.UpdateTodoAsync(1, It.IsAny<Todo>()))
                           .ReturnsAsync(updatedTodo);

            // Act
            var result = await _todoService.UpdateTodoAsync(1, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(TodoMapper.ToDto(updatedTodo));
        }

        [Fact]
        public async Task UpdateTodoAsync_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            //Arrange
            var existingTodo = new Todo
            {
                Id = 1,
                Title = "Old Title",
                Status = Status.InProgress,
                Description = "Old Description",
                IsDeleted = false
            };
            var updateRequest = new TodoRequest
            {
                Title = "New Title",
                Status = Status.Done,
                Description = "New Description"
            };
            var updatedTodo = new Todo
            {
                Id = 1,
                Title = updateRequest.Title,
                Status = updateRequest.Status,
                Description = updateRequest.Description,
                IsDeleted = false
            };
            _todoRepository.Setup(x => x.UpdateTodoAsync(1, It.IsAny<Todo>())).ReturnsAsync(updatedTodo);

            //Act
            var result = await _todoService.UpdateTodoAsync(2, updateRequest);

            // Assert
            result.Should().BeNull();
            result.Should().NotBeEquivalentTo(TodoMapper.ToDto(updatedTodo));
        }

        [Fact]
        public async Task DeleteTodoAsync_ReturnsUpdatedTodo_WhenTodoExists()
        {
            var existingTodo = new Todo
            {
                Id = 1,
                Title = "Old Title",
                Status = Status.InProgress,
                Description = "Old Description",
                IsDeleted = false
            };
            _todoRepository.Setup(x => x.DeleteTodoAsync(existingTodo.Id)).ReturnsAsync(existingTodo);

            //Act
            var result = await _todoService.DeleteTodoAsync(existingTodo.Id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(TodoMapper.ToDto(existingTodo));
        }

        [Fact]
        public async Task UpdateTodoAsync_ThrowsException_WhenTitleIsNull()
        {
            //arrange
            var todo = new TodoRequest
            {
                Title = "",
                Status = Status.InProgress,
                Description = "Old Description",
            };
            _todoRepository.Setup(x => x.UpdateTodoAsync(1, It.IsAny<Todo>())).ReturnsAsync(It.IsAny<Todo>());

            //act & assert
            await FluentActions.Awaiting(() => _todoService.UpdateTodoAsync(1, todo))
         .Should()
         .ThrowAsync<ArgumentException>()
         .WithMessage("Title is required");


        }
    }
}