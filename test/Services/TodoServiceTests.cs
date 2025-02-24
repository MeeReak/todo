using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using api.Repository.Interface;
using api.Services;
using api.Models;
using api.Helper;
using api.Dtos.Todo;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public async Task GetTodoAsync_ShouldReturnTodo_WhenTodoExisted()
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
        public async Task GetTodosAsync_ShouldReturnTodo_WhenTodoExisted()
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
        public async Task GetTodosAsync_ShouldCreateTodo_WhenTodoExisted()
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
        public async Task UpdateTodoAsync_ShouldReturnUpdatedTodo_WhenTodoExists()
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
        public async Task DeleteTodoAsync_ShouldReturnUpdatedTodo_WhenTodoExists()
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

    }
}