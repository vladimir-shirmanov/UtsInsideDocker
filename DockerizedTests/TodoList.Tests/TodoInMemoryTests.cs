using Microsoft.AspNetCore.Http.HttpResults;
using TodoList.Api;
using TodoList.Tests.Utils;

namespace TodoList.Tests;

public class TodoInMemoryTests
{
    [Fact]
    public async Task GetTodos_ReturnsTodosFromDb()
    {
        //arrange
        await using var context = new MockDb().CreateDbContext();
        context.Todos.Add(new Todo
        {
            Id = 1,
            Name = "test 1",
            IsCompleted = false
        });
        
        context.Todos.Add(new Todo
        {
            Id = 2,
            Name = "test 2",
            IsCompleted = true
        });
        await context.SaveChangesAsync();
        
        //act
        var result = await TodoEndpoints.GetTodos(context);
        
        //assert
        Assert.IsType<Ok<List<Todo>>>(result);

        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Collection(result.Value, todo1 =>
        {
            Assert.Equal(1, todo1.Id);
            Assert.Equal("test 1", todo1.Name);
            Assert.False(todo1.IsCompleted);
        }, todo2 =>
        {
            Assert.Equal(2, todo2.Id);
            Assert.Equal("test 2", todo2.Name);
            Assert.True(todo2.IsCompleted);
        });
    }
    
    [Fact]
    public async Task CreateTodoCreatesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        var newTodo = new Todo
        {
            Name = "Test title",
            IsCompleted = false
        };

        //Act
        var result = await TodoEndpoints.CreateTodo(newTodo, context);

        //Assert
        Assert.IsType<Created<Todo>>(result);

        Assert.NotNull(result);
        Assert.NotNull(result.Location);

        Assert.NotEmpty(context.Todos);
        Assert.Collection(context.Todos, todo =>
        {
            Assert.Equal("Test title", todo.Name);
            Assert.False(todo.IsCompleted);
        });
    }
    
    [Fact]
    public async Task GetTodoReturnsNotFoundIfNotExists()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        // Act
        var result = await TodoEndpoints.GetTodo(1, context);

        //Assert
        Assert.IsType<Results<Ok<Todo>, NotFound>>(result);

        var notFoundResult = (NotFound) result.Result;

        Assert.NotNull(notFoundResult);
    }
    
    [Fact]
    public async Task GetTodoReturnsTodoFromDatabase()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new Todo
        {
            Id = 1,
            Name = "Test title",
            IsCompleted = false
        });

        await context.SaveChangesAsync();

        // Act
        var result = await TodoEndpoints.GetTodo(1, context);

        //Assert
        Assert.IsType<Results<Ok<Todo>, NotFound>>(result);

        var okResult = (Ok<Todo>)result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(1, okResult.Value.Id);
    }
}