/**
 * In the Working with DbContext chapter, we used the context.Database.EnsureCreated() method to create the database and schema for the first time.
 * Note that it creates a database first time only. It cannot change the DB schema after that.
 * For the developement projects, we must use EF Core Migrations API.
 * To use EF Core Migrations API, we need to install the NuGet package Microsoft.EntityFrameworkCore.Tools and Microsoft.EntityFrameworkCore.Relational.
 * We use EF Core 7.0.14, so install the same version of package.
 */
using Microsoft.EntityFrameworkCore;

namespace SharedService;

public static class DbInitilializer
{
    public static void Migrate<T>(IServiceProvider serviceProvider) where T : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();
        context.Database.Migrate();
    }
}
