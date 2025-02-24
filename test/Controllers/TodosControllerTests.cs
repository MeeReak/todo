using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using api.Models;
using api.Services;
using api.Controllers;
using api.Helper;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Todo;

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
        public async Task GetTodos_ShouldReturnOk_WhenTodosExist()
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
                new() {
                    Id = 2,
                    Title = "Unit Test with C#",
                    Status = Status.InProgress,
                    Description = "Write a unit test then demo",
                    IsDeleted = false
                }
            };
            _mockTodoService.Setup(x => x.GetTodosAsync()).ReturnsAsync([.. todos.Select(TodoMapper.ToDto)]);

            //Act
            var result = await _todoController.GetTodos();


            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(todos.Select(TodoMapper.ToDto));
        }

        [Fact]
        public async Task GetTodo_ShouldReturnOk_WhenTodoExisted()
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
        public async Task GetTodo_ShouldReturnNotFound_WhenTodoNotExisted()
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
        public async Task AddTodo_ShouldReturnTodo_WhenDone()
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

    }
}