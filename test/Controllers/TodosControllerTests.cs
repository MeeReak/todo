using api.Controllers;
using api.Dtos.Todo;
using api.Helper;
using api.Mappers;
using api.Models;
using api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace test.Controllers
{
    public class TodosControllerTests
    {
        private readonly Mock<ITodoService> _mockTodoService;
        private readonly TodoController _todoController;
        public TodosControllerTests()
        {
            _mockTodoService = new Mock<ITodoService>();
            _todoController = new TodoController(_mockTodoService.Object);
        }

        [Fact]
        public async Task GetTodos_ReturnsOkResult_WhenTodosExist()
        {
            //Arrange
            var todos = new List<Todo>{
                new() {
                    Id = 1,
                    Title = "Unit Test with C#",
                    Status = Status.InProgress,
                    Description = "Write a unit test then demo",
                    IsDeleted = false
                },

            };
            _mockTodoService.Setup(x => x.GetTodosAsync()).ReturnsAsync([.. todos.Select(TodoMapper.ToDto)]);

            //Act
            var result = await _todoController.GetTodos();


            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(todos.Select(TodoMapper.ToDto));
        }

        [Fact]
        public async Task GetTodo_ReturnsOkResult_WhenTodoExists()
        {
            //Arrange
            var todo = new Todo
            {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            };
            _mockTodoService.Setup(x => x.GetTodoAsync(1)).ReturnsAsync(TodoMapper.ToDto(todo));

            //Act
            var result = await _todoController.GetTodo(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(TodoMapper.ToDto(todo));
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<TodoDto>().Which.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetTodo_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            //Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _ = _mockTodoService.Setup(x => x.GetTodoAsync(1)).ReturnsAsync((TodoDto)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //Act
            var result = await _todoController.GetTodo(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddTodo_ReturnsCreatedTodo_WhenSuccessful()
        {
            //Arrange
            var todo = new TodoRequest
            {
                Title = "Testing Code",
                Description = "The Code is Testing Me",
                Status = Status.ToDo,
            };
            var todoResponse = new TodoDto
            {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            };
            _mockTodoService.Setup(x => x.AddTodoAsync(todo)).ReturnsAsync(todoResponse);

            //Act
            var result = await _todoController.AddTodo(todo);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(todoResponse);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<TodoDto>().Which.Title.Should().Be("Unit Test with C#");

        }

        [Fact]
        public async Task UpdateTodo_ReturnsUpdatedTodo_WhenSuccessful()
        {
            var todo = new Todo
            {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            };
            //arrange
            var todoRequest = new TodoRequest
            {
                Title = "Testing Code",
                Description = "The Code is Testing Me",
                Status = Status.ToDo,
            };
            var updated = new TodoDto
            {
                Id = 1,
                Title = "Testing Code",
                Description = "The Code is Testing Me",
                Status = Status.ToDo,
                IsDeleted = false
            };
            _mockTodoService.Setup(x => x.UpdateTodoAsync(1, todoRequest)).ReturnsAsync(updated);

            //act
            var result = await _todoController.UpdateTodo(1, todoRequest);

            //assert
            result.Should().NotBeNull();
            result.Should().NotBeEquivalentTo(todo);
        }

        [Fact]
        public async Task DeleteTodo_ReturnsOkResult_WhenTodoExists()
        {
            //arrange
            var todo = new Todo
            {
                Id = 1,
                Title = "Unit Test with C#",
                Status = Status.InProgress,
                Description = "Write a unit test then demo",
                IsDeleted = false
            };
            _mockTodoService.Setup(x => x.DeleteTodoAsync(1)).ReturnsAsync(TodoMapper.ToDto(todo));

            //act
            var result = await _todoController.DeleteTodo(1);

            //assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteTodo_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            // Arrange
            _ = _mockTodoService.Setup(x => x.DeleteTodoAsync(2)).ReturnsAsync((TodoDto?)null);

            // Act
            var result = await _todoController.DeleteTodo(2);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}