using Microsoft.EntityFrameworkCore;
using TodoList.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("Todos"));
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGroup("/todos")
    .MapTodosApi()
    .WithOpenApi();

app.Run();