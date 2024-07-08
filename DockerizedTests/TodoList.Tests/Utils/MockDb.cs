using Microsoft.EntityFrameworkCore;
using TodoList.Api;

namespace TodoList.Tests.Utils;

public class MockDb : IDbContextFactory<TodoDb>
{
    public TodoDb CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDb>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;
        return new TodoDb(options);
    }
}