using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Api;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodosApi(this RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/", GetTodos);
        groupBuilder.MapGet("/{id}", GetTodo);
        groupBuilder.MapPost("/", CreateTodo);
        return groupBuilder;
    }

    public static async Task<Ok<List<Todo>>> GetTodos([FromServices] TodoDb db)
    {
        var todos = await db.Todos.ToListAsync();
        return TypedResults.Ok(todos);
    }

    public static async Task<Results<Ok<Todo>, NotFound>> GetTodo(int id, [FromServices] TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(todo);
    }

    public static async Task<Created<Todo>> CreateTodo([FromBody] Todo todo, [FromServices] TodoDb db)
    {
        await db.AddAsync(todo);
        await db.SaveChangesAsync();

        return TypedResults.Created($"{todo.Id}", todo);
    }
}